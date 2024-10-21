using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IFriendshipService
    {
        public Task DeleteUserFriendshipsAsync(string userId);

        public Task<IEnumerable<Friendship>> GetUserFriendshipsAsync(string userId);
    }
}
