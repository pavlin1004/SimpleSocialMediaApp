using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.InputModels.Messages;
using SimpleSociaMedialApp.Services.Functional.Interfaces;

namespace SimpleSocialApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<AppUser> _userManager;
        public MessageController(IMessageService messageService,IChatService chatService, IHubContext<ChatHub> hubContext, UserManager<AppUser> userManager)
        {
            _messageService = messageService;
            _chatService = chatService;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        [HttpPost]   
        public async Task<IActionResult> Send([FromBody] MessageInputModel model)
        {           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            var chat = await _chatService.GetChatAsync(model.ChatId);

            if(chat == null)
            {
                return NotFound();
            }
            var message = new Message
            {
                UserId = user.Id,
                ChatId = model.ChatId,
                CreatedDateTime = DateTime.UtcNow,
                Content = model.Content
            };     

            await _messageService.CreateMessageAsync(message);
            await _hubContext.Clients.Group(model.ChatId).SendAsync("ReceiveMessage",user.FirstName, message.Content, message.CreatedDateTime.ToString("HH:mm:ss"));

            return BadRequest();
        }
    }
}
