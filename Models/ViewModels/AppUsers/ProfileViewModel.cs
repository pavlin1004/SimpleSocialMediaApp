using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.ViewModels.Posts;

namespace SimpleSociaMedialApp.Models.ViewModels.AppUsers
{
    public class ProfileViewModel
    {
        public required AppUser User { get; set; }

        public Friendship? FriendshipStatus { get; set; }

        public required IEnumerable<PostViewModel> Posts { get; set; }

        public bool IsCurrentUser { get; set; }
    }
}
