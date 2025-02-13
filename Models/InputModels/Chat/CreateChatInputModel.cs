using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Models.InputModels.Chat
{
    public class CreateChatInputModel
    {
        public required string Title { get; set; }
        public required string OwnerId { get; set; }
        public List<AppUser>? Participants { get; set; }
    }
}
