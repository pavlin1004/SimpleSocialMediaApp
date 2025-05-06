using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.ViewModels.Chats;
using SimpleSociaMedialApp.Services.Functional.Interfaces;
using SimpleSociaMedialApp.Services.Utilities.Interfaces;
using System.Security.Claims;

namespace SimpleSocialApp.ViewComponents
{
    public class ListChatsViewComponent : ViewComponent
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ListChatsViewComponent(IUserService userService, IChatService chatService, IMapper mapper, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _chatService = chatService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            var chats = await _chatService.OrderChatsByLastMessages(user.Id,10);

            var chatViewModelList = new List<ChatViewModel>();
            foreach (var chat in chats)
            {
                if (chat.Type == ChatType.Group)
                {
                    chatViewModelList.Add(_mapper.MapToChatViewModel(chat,0,0));
                }
                else
                {
                    var friendId = chat.Users.Where(u => u.Id != user.Id).Select(u => u.Id).First();
                    var friend = await _userService.GetUserByIdAsync(friendId);
                    chatViewModelList.Add(_mapper.MapToChatViewModel(chat, friend,0,0));
                }
            }
            return View(chatViewModelList);
        }
    }
}
