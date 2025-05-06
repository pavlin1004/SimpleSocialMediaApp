using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.InputModels.Chat;
using SimpleSocialApp.Models.ViewModels.Chats;
using SimpleSociaMedialApp.Services.Functional.Interfaces;
using SimpleSociaMedialApp.Services.Utilities.Interfaces;
using System.Security.Claims;

namespace SimpleSocialApp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IFriendshipService _friendshipService;
        private readonly IUserService _userService; // Needed for fetching user objects
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
       

        public ChatController(IChatService chatService,
                              IFriendshipService friendshipService,
                              IUserService userService,
                              IMapper mapper,
                              UserManager<AppUser> userManager)
        {
            _chatService = chatService;
            _friendshipService = friendshipService;
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;            
        }

        public async Task<IActionResult> Index(string chatId, int count = 0, int size = 5)
        {
            var user = await _userManager.GetUserAsync(User);
            var chat = await _chatService.GetChatAsync(chatId);
            if (chat == null)
            {
                return NotFound();
            }
            if (chat.Type == ChatType.Group)
            {
                if (count == 0)
                {
                    return View(_mapper.MapToChatViewModel(chat, count, size));
                }
                else
                {
                    return PartialView("Message/_MessagesPartial", _mapper.MapToChatViewModel(chat, count, size));
                }
            }
            else
            {
                var friendId = chat.Users.Where(u => u.Id != user.Id).Select(u => u.Id).First();
                var friend = await _userService.GetUserByIdAsync(friendId);
                if (count == 0) 
                {
                    return View(_mapper.MapToChatViewModel(chat, friend, count, size));
                }
                else
                {
                    return PartialView("Message/_MessagesPartial", _mapper.MapToChatViewModel(chat, friend, count, size));
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListChats(string userId, string searchQuery = "")
        {
            if(string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home"); 
            }
            var chats = new List<Chat>();

            if (searchQuery == "") chats = await _chatService.GetChatsForUserAsync(userId);
            else chats = await _chatService.SearchChatAsync(userId, searchQuery);

            var chatViewModelList = new List<ChatViewModel>();

            foreach(var chat in chats)
            {
                if (chat.Type == ChatType.Group)
                {
                    chatViewModelList.Add(_mapper.MapToChatViewModel(chat,0,0));
                }
                else
                {
                    var friendId = chat.Users.Where(u => u.Id != userId).Select(u => u.Id).First();
                    var friend = await _userService.GetUserByIdAsync(friendId);
                    chatViewModelList.Add(_mapper.MapToChatViewModel(chat, friend, 0,0));
                }
            }
            return View(chatViewModelList);           
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var friends = await _friendshipService.GetAllFriends(currentUser.Id);
            if (friends == null || friends.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No users available to create a chat.");
                return RedirectToAction("Index", "Home");
            }
            var chat = new CreateChatViewModel
            {
                Users = friends
            };
            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateChatInputModel model)
        {
            if (!ModelState.IsValid)
            {
               return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            var chat = new Chat
            {
                OwnerId = user.Id,
                Title = model.Title,
                Type = ChatType.Group,
                CreatedDateTime = DateTime.Now,
            };
;
            foreach (var participantId in model.ParticipantsIds)
            {
                var participant = await _userService.GetUserByIdAsync(participantId);
                if (participant != null)
                {
                    chat.Users.Add(participant);
                }
            }
            chat.Users.Add(user);
            await _chatService.CreateChatAsync(chat);

            return RedirectToAction("ListChats", new { userId = user.Id});
        }
        [HttpGet]
        public async Task<IActionResult> ModifyUsers(string chatId, string userId, string actionType)
        {
            var chat = await _chatService.GetChatAsync(chatId);
            if(chat == null)
            {
                return BadRequest();
            }
            ModifyChatParticipantViewModel model;
            if (actionType == "Remove")
            {
                var participants = await _chatService.GetAllUsersAsync(chat);
                var filteredParticipants = participants.Where(p => p.Id != userId).ToList();
                model = new ModifyChatParticipantViewModel
                {
                    ChatId = chatId,
                    Users = filteredParticipants,
                    Action = "Remove"
                };
            }
            else
            {
                var friends = await _friendshipService.GetAllFriends(userId);
                var friendsNotInChat = friends.Where(f => !chat.Users.Any(u => u.Id == f.Id)).ToList();
                model = new ModifyChatParticipantViewModel
                {
                    ChatId = chatId,
                    Users = friendsNotInChat,
                    Action = "Add"
                };
                if (model.Users == null) throw new Exception("null to add");
            }
            return View(model);
        }
       
        [HttpPost]
        public async Task<IActionResult> ModifyUsers(string chatId, List<string> userIds, string actionType)
        {              
            if(userIds == null || userIds.Count==0)
            {
                RedirectToAction("Index", "Chat", new { ChatId = chatId });
            }
            var chat = await _chatService.GetChatAsync(chatId);
            if (chat == null) return NotFound();
            if (actionType == "Add")
            {
                foreach (var userId in userIds)
                {
                    var userToAdd = await _userService.GetUserByIdAsync(userId);
                    if (userToAdd != null)
                    {
                        await _chatService.AddUserAsync(chat, userToAdd);
                    }
                }
            }
            else if(actionType == "Remove")
            {
                foreach (var userId in userIds)
                {
                    var userToAdd = await _userService.GetUserByIdAsync(userId);
                    if (userToAdd != null)
                    {
                        await _chatService.RemoveUserAsync(chat, userToAdd);
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid operation!";
            }
            return RedirectToAction("Index", "Chat", new { ChatId = chatId });
        }

        public async Task<IActionResult> DeleteChat(string chatId)
        {
            var user = await _userManager.GetUserAsync(User);

            var chat = await _chatService.GetChatAsync(chatId);
            if (chat == null) return NotFound();

            if (user.Id != chat.OwnerId) return Unauthorized();            

            await _chatService.DeleteChatAsync(chatId);

            return RedirectToAction("ListChats", new { userId = user.Id});
        }

        public async Task<IActionResult> LeaveChat(string chatId)
        {
            var user = await _userManager.GetUserAsync(User);
            var chat = await _chatService.GetChatAsync(chatId);

            if (user == null) return Challenge();
            if (chat == null) return NotFound("Chat not found");            

            bool result = await _chatService.RemoveUserAsync(chat, user);

            if(!result)
            {
                return BadRequest($"Chat with id {chatId} doesn't contain user with id {user.Id}");
            }
            return RedirectToAction("ListChats", "Chat", new { userId = user.Id });         
        }

      
    }
}
