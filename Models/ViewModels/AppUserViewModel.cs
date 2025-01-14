using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Models.ViewModels
{
    public class AppUserViewModel
    {
        public required AppUser User { get; set; }

        public Friendship? FriendshipStatus { get; set; }

        public required IEnumerable<Post?> Posts { get; set; }

        public bool IsCurrentUser { get; set; }
    }
}
