using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.ViewModels.Chats;

namespace SimpleSocialApp.Mapping
{
    public class Mapper : IMapper
    {
        public ChatViewModel MapToChatViewModel(Chat chat, string friendName, int count, int size)
        {
            return new ChatViewModel
            {
                ChatId = chat.Id,
                Messages = chat.Messages?.OrderByDescending(c => c.CreatedDateTime).Skip(size * count)
                .Take(size).ToList() ?? new List<Message>(),
                Type = chat.Type,
                FriendName = friendName,
                Title = null,
                OwnerId = null
            };
        }
        public ChatViewModel MapToChatViewModel(Chat chat, int count, int size)
        {
            return new ChatViewModel
            {
                ChatId = chat.Id,
                Messages = chat.Messages?.OrderByDescending(c => c.CreatedDateTime).Skip(size * count)
                .Take(size).ToList() ?? new List<Message>(),
                Type = chat.Type,
                FriendName = null,
                Title = chat.Title,
                OwnerId = chat.OwnerId
            };
        }

    }
}
