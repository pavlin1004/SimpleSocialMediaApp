using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.InputModels.Chat;
using SimpleSocialApp.Models.ViewModels.Chat;
using SimpleSocialApp.Services.Implementations;
using SimpleSocialApp.Services.Interfaces;
using System.Security.Claims;

namespace SimpleSocialApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IFriendshipService _friendshipService;

        public ChatController(ChatService chatService, FriendshipsService friendshipSerivce)
        {
            _chatService = chatService;
            _friendshipService = friendshipSerivce;  
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if(userId != null)
            {
                var friends = await _friendshipService.GetAllFriends(userId);
                CreateChatViewModel model = new CreateChatViewModel
                {
                    OwnerId = userId,
                    Participants = friends
                };

                return View(model);
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateChatInputModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var Chat = new Chat
            {
                OwnerId = model.OwnerId,
                Title=  model.Title,
                CreatedOn = DateTime.Now,
                Users = model.Participants
            };

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> RemoveUsersFromChat(string chatId, List<string> userIds)
        {

        }
        public async Task<IActionResult> AddUsersToChat(string chatId, List<string> userIds)
        {

        }
        public async Task<IActionResult> DeleteChat(string chatId, string userId)
        {

        }
    }
}
