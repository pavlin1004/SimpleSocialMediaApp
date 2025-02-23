using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Models.ViewModels
{
    public class ChatIndexViewModel
    {
        public required string Title { get; set; }

        public string? OwnerId { get; set; }

        public required string ChatId { get; set; }

        public List<Message>? Messages { get; set; }
    }
}
