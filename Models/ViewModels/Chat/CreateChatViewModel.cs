using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Models.ViewModels.Chat
{
    public class CreateChatViewModel
    {
        public required string OwnerId { get; set; }

        public List<AppUser> Participants { get; set; }
    }
}
