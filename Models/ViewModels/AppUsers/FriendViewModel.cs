namespace SimpleSocialApp.Models.ViewModels.AppUsers
{
    public class FriendViewModel
    {

        public required string UserId { get; set; } //Id of the user with the current friend list
        public List<UserViewModel>? Friends { get; set; }

    }
}
