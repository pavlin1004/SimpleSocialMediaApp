using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services
{
    public interface IFriendService 
    {

        public Task AddAsync(Friendship f);

        public Task DeleteAsync(Friendship f);

        public Task<ICollection<string>> GetAllFriendsIdsAsync(string userId);


    }
}
