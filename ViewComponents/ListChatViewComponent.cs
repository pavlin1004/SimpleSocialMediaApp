using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;

        public ListChatViewComponent(IChatService chatService, IMapper mapper)
        {
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
            var chats = await _chatService.GetConversationsForUserAsync(currentUserId);

            var chatViewModelList = new List<ChatViewModel>();
            foreach (var chat in chats)
            {
                if (chat.Type == ChatType.Group)
                {
                    chatViewModelList.Add(_mapper.MapToChatViewModel(chat));
                }
                else
                {
                    var friendName = _chatService.GetFriendName(chat, currentUserId);
                    chatViewModelList.Add(_mapper.MapToChatViewModel(chat, friendName));
                }
            }
            return View(chatViewModelList);
        }
    }
}
