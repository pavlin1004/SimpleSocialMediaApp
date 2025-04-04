using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Mapping;
using SimpleSocialApp.Models.ViewModels.Chats;
using SimpleSocialApp.Services.Interfaces;
using System.Security.Claims;

namespace SimpleSocialApp.ViewComponents
{
    public class ListChatViewComponent : ViewComponent
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ListChatViewComponent(IUserService userService, IChatService chatService, IMapper mapper)
        {
            _userService = userService;
            _chatService = chatService;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = (User as ClaimsPrincipal)?.FindFirstValue(ClaimTypes.NameIdentifier);
            if(String.IsNullOrEmpty(currentUserId))
            {
                throw new Exception("1111111111111111111111111111111111111111111111111111111");
            }
            var chats = _chatService.GetLast(currentUserId,10);

            var chatViewModelList = new List<ChatViewModel>();
            foreach (var chat in chats)
            {
                if (chat.Type == ChatType.Group)
                {
                    chatViewModelList.Add(_mapper.MapToChatViewModel(chat,0,0));
                }
                else
                {
                    var friendId = chat.Users.Where(u => u.Id != currentUserId).Select(u => u.Id).First();
                    var friend = await _userService.GetUserByIdAsync(friendId);
                    chatViewModelList.Add(_mapper.MapToChatViewModel(chat, friend,0,0));
                }
            }
            return View(chatViewModelList);
        }
    }
}
