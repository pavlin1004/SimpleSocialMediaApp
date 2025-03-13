using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.InputModels.Messages;
using SimpleSocialApp.Services.Interfaces;
using System.Security.Claims;

namespace SimpleSocialApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly IHubContext<ChatHub> _hubContext;
        public MessageController(IMessageService messageService,IChatService chatService, IUserService userService, IHubContext<ChatHub> hubContext)
        {
            _messageService = messageService;
            _chatService = chatService;
            _userService = userService;
            _hubContext = hubContext;
        }

        [HttpPost]   
        public async Task<IActionResult> Send([FromBody] MessageInputModel model)
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

            var chat = await _chatService.GetConversationAsync(model.ChatId);

            if(chat == null)
            {
                return NotFound();
            }
            var message = new Message
            {
                UserId = currentUserId,
                ChatId = model.ChatId,
                CreatedDateTime = DateTime.UtcNow,
                Content = model.Content
            };

            
            var user = await  _userService.GetUserByIdAsync(currentUserId);

            await _messageService.CreateMessageAsync(message);

            await _hubContext.Clients.Group(model.ChatId).SendAsync("ReceiveMessage",user.FirstName, message.Content, message.CreatedDateTime.ToString("HH:mm:ss"));

            return BadRequest();
        }
    }
}
