using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.ViewModels.Chats;

namespace SimpleSocialApp.Mapping
{
    public class Mapper : IMapper
    {
        public ChatViewModel MapToChatViewModel(Chat chat, string friendName)
        {
            return new ChatViewModel
            {
                ChatId = chat.Id,
                Messages = chat.Messages?.OrderBy(c => c.TimeSent).ToList() ?? new List<Message>(),
                Type = chat.Type,
                FriendName = friendName,
                Title = null,
                OwnerId = null
            };
        }
        public ChatViewModel MapToChatViewModel(Chat chat)
        {
            return new ChatViewModel
            {
                ChatId = chat.Id,
                Messages = chat.Messages?.OrderBy(c => c.TimeSent).ToList() ?? new List<Message>(),
                Type = chat.Type,
                FriendName = null,
                Title = chat.Title,
                OwnerId = chat.OwnerId
            };
        }

    }
}
