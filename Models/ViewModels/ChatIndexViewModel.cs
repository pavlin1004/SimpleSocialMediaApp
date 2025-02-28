using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Models.ViewModels
{
    public class ChatIndexViewModel
    {
        public string? OtherUser { get; set; }
        public string? Title { get; set; }

        public string? OwnerId { get; set; }

        public bool IsGroup { get; set; }
        public required string ChatId { get; set; }

        public List<Message>? Messages { get; set; }
    }
}
