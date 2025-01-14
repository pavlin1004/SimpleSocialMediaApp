using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IFriendshipService
    {

        public Task<Friendship?> GetFriendshipById(string senderId, string receiverId);
        public Task SendFriendshipRequestAsync(string senderId, string receiverId);
        public Task RemoveUserFriendshipsAsync(string senderId, string receiverId);
        public Task AcceptUserFriendshipAsync(string senderId, string receiverId);

        public Task<IEnumerable<Friendship>> GetUserAcceptedFriendshipsAsync(string userId);
        public Task<IEnumerable<Friendship>> GetUserPendingFriendshipsAsync(string userId);

        public Task<Friendship?> CheckFriendship(string user1Id, string user2Id);
    }
}
