using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Mapping;
using SimpleSocialApp.Models.InputModels.Chat;
using SimpleSocialApp.Models.ViewModels.Chats;
using SimpleSocialApp.Services.Interfaces;
using System.Security.Claims;

namespace SimpleSocialApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IFriendshipService _friendshipService;
        private readonly IUserService _userService; // Needed for fetching user objects
        private readonly IMapper _mapper;
       

        public ChatController(IChatService chatService, IFriendshipService friendshipService, IUserService userService, IMapper mapper)
        {
            _chatService = chatService;
            _friendshipService = friendshipService;
            _userService = userService;
            _mapper = mapper;

        }

        public async Task<IActionResult> Index(string chatId, int count = 0, int size = 5)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }
            var chat = await _chatService.GetConversationAsync(chatId);
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
                var friendId = chat.Users.Where(u => u.Id != currentUserId).Select(u => u.Id).First();
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
        public async Task<IActionResult> ListChat(string userId, string searchQuery = "")
        {
            if(string.IsNullOrEmpty(userId))
            {
                RedirectToAction("Index", "Home"); 
            }
            var chats = new List<Chat>();
            if (searchQuery == "")
            {
                chats = await _chatService.GetConversationsForUserAsync(userId);
            }
            else
            {
                chats = await _chatService.SearchChat(userId, searchQuery);
            }
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var friends =  await _friendshipService.GetAllFriends(userId);
            if(friends == null || friends.Count == 0)
            {
                TempData["ErrorMessage"] = "No users available to create a chat.";
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

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }
            var currentUser = await _userService.GetUserByIdAsync(currentUserId);
            if (currentUser == null)
            {
                return BadRequest();
            }

            var chat = new Chat
            {
                OwnerId = currentUserId,
                Title = model.Title,
                Type = ChatType.Group,
                CreatedDateTime = DateTime.Now,
            };
;
            foreach (var participantId in model.ParticipantsIds)
            {
                var user = await _userService.GetUserByIdAsync(participantId);
                if (user != null)
                {
                    chat.Users.Add(user);
                }
            }   
            chat.Users.Add(currentUser);
         
            await _chatService.CreateConversationAsync(chat);

            return RedirectToAction("ListChat", new { userId = currentUserId});
        }
        [HttpGet]
        public async Task<IActionResult> ModifyUsers(string chatId, string userId, string actionType)
        {
            var chat = await _chatService.GetConversationAsync(chatId);
            if(chat == null)
            {
                return BadRequest();
            }
            ModifyChatParticipantViewModel model;
            //if (string.IsNullOrEmpty(actionType))
            //{
            //    throw new Exception($"null string {actionType}");
            //}
            //if (!string.IsNullOrEmpty(chatId))
            //{
            //    throw new Exception($"null string {chatId}");
            //}
            //if (chat == null)
            //{
            //    throw new Exception($"null string {actionType}");
            //}
            if (actionType == "Remove")
            {
                var participants = await _chatService.GetAllUsers(chat);
                var filteredParticipants = participants.Where(p => p.Id != userId).ToList();
                model = new ModifyChatParticipantViewModel
                {
                    ChatId = chatId,
                    Users = filteredParticipants,
                    Action = "Remove"
                };
                //if (model.Users == null) throw new Exception("null to add");
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
        //public async Task<IActionResult> RemoveUsersFromChat(string chatId, List<string> userIds)
        //{
        //    var chat = await _chatService.GetConversationAsync(chatId);
        //    var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (chat == null || userIds == null || userIds.Count == 0)
        //    {
        //        return BadRequest();
        //    }

        //    if (currentUserId != chat.OwnerId)
        //    {
        //        return Unauthorized();
        //    }

        //    foreach (var userId in userIds)
        //    {
        //        var user = await _userService.GetUserByIdAsync(userId);
        //        if (user != null)
        //        {
        //            await _chatService.RemoveUserAsync(chat, user);
        //        }
        //    }

        //    return RedirectToAction("Index", new { chatId });
        //}
        //[HttpGet]
        //public async Task<IActionResult> AddUsersToChat(string chatId, string userId)
        //{
        //    if(string.IsNullOrEmpty(chatId) || string.IsNullOrEmpty(userId))
        //    {
        //        return BadRequest();
        //    }
        //    var chat = await _chatService.GetConversationAsync(chatId);
        //    var friends = await _friendshipService.GetAllFriends(userId);
        //    var toAdd = friends.Where(f => !chat.Users.Any(u => u.Id == f.Id));
        //   //if(toAdd = null || )
        //    var model = new ModifyChatParticipantViewModel
        //    {
        //        ChatId = chatId,
        //        Users = toAdd
        //    };

        //    return View(model); 
        //}
        [HttpPost]
        public async Task<IActionResult> ModifyUsers(string chatId, List<string> userIds, string actionType)
        {              
            var chat = await _chatService.GetConversationAsync(chatId);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (chat == null || userIds == null || userIds.Count == 0)
            {
                return BadRequest("Something is null");
            }

            if (currentUserId != chat.OwnerId)
            {
                return Unauthorized();
            }

            if (actionType == "Add")
            {
                foreach (var userId in userIds)
                {
                    var user = await _userService.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        await _chatService.AddUserAsync(chat, user);
                    }
                }
            }
            else if(actionType == "Remove")
            {
                foreach (var userId in userIds)
                {
                    var user = await _userService.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        await _chatService.RemoveUserAsync(chat, user);
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
            var chat = await _chatService.GetConversationAsync(chatId);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (chat == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(currentUserId) || currentUserId != chat.OwnerId)
            {
                return Unauthorized();
            }

            await _chatService.DeleteConversationAsync(chatId);

            return RedirectToAction("ListChat", new { userId = currentUserId});
        }

        public async Task<IActionResult> LeaveChat(string chatId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }
            if(string.IsNullOrEmpty(chatId))
            {
                return BadRequest();
            }

            var chat = await _chatService.GetConversationAsync(chatId);

            if (chat == null)
            {
                return NotFound("Chat not found");
            }

            var user = await _userService.GetUserByIdAsync(currentUserId);

            if(user == null)
            {
                return NotFound("User not found!");
            }
            bool result = await _chatService.RemoveUserAsync(chat, user);

            if(result == false)
            {
                return BadRequest($"Chat with id {chatId} doesn't contain user with id {user.Id}");
            }
            return RedirectToAction("ListChat", "Chat", new { userId = currentUserId });         
        }

        //public async Task<IActionResult> CheckMembers(string chatId)
        //{
        //    var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(currentUserId))
        //    {
        //        return Unauthorized();
        //    }
        //    if (string.IsNullOrEmpty(chatId))
        //    {
        //        return BadRequest();
        //    }

        //    var chat = await _chatService.GetConversationAsync(chatId);

        //    if (chat == null)
        //    {
        //        return NotFound("Chat not found");
        //    }

        //    var user = await _userService.GetUserByIdAsync(currentUserId);

        //    if (user == null)
        //    {
        //        return NotFound("User not found!");
        //    }
            

        //    if (result == false)
        //    {
        //        return BadRequest($"Chat with id {chatId} doesn't contain user with id {user.Id}");
        //    }
        //    return RedirectToAction("ListChat", "Chat", new { userId = currentUserId });
        //}
    }
}
