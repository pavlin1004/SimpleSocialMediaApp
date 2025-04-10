using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Models.ViewModels.Chats
{
    public class ModifyChatParticipantViewModel
    {
        public required string ChatId { get; set; }

        public required List<AppUser>? Users { get; set; }

        public required string Action { get; set; }
    }
}
