using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IFriendshipService
    {

        public Task<Friendship?> GetFriendshipById(string senderId, string receiverId);
        public Task SendFriendshipRequestAsync(string senderId, string receiverId);
        public Task RemoveUserFriendshipsAsync(string senderId, string receiverId);
        public Task AcceptUserFriendshipAsync(string senderId, string receiverId);

        public Task<List<Friendship>> GetUserAcceptedFriendshipsAsync(string userId);
        public Task<List<Friendship>> GetUserPendingFriendshipsAsync(string userId);

        public Task<Friendship?> CheckFriendship(string user1Id, string user2Id);
        public Task<List<AppUser>> GetAllFriends(string userId);

        public Task<List<string>> GetAllFriendsIds(string userId);

        public Task<(List<AppUser>, List<AppUser>)> GetNonFriendUsers(string userId);

        public Task CreateAsync(Friendship f);
    }
}
