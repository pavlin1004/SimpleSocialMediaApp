using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Models.ViewModels.Chats
{
    public class ChatViewModel
    {
        public string? Title { get; set; }

        public string? OwnerId { get; set; }

        public AppUser? Friend { get; set; }
        public required ChatType Type { get; set; }
        public required string ChatId { get; set; }

        public List<Message>? Messages { get; set; }
    }
}
