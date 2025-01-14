using Microsoft.AspNetCore.SignalR;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        //public async Task SendMessage(int chatId, string senderId, string messageContent)
        //{
        //    // Logic for sending messages
        //}

        //public async Task JoinChat(int chatId)
        //{
        //    // Logic for joining a SignalR group
        //}

        //public async Task LeaveChat(int chatId)
        //{
        //    // Logic for leaving a SignalR group
        //}
    }
}
