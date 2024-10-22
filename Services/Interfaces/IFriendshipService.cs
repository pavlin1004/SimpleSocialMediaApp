using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IFriendshipService
    {

        public Task<Friendship?> GetFriendshipById(string Id);


        public Task CreateFriendshipRequestAsync(Friendship f);


        public Task<bool> RejectUserFriendshipsAsync(string userId);



        public Task<bool> AcceptUserFriendshipAsync(string friendshipid);



        public Task<IEnumerable<Friendship>> GetUserAcceptedFriendshipsAsync(string userId);



        public Task<IEnumerable<Friendship>> GetUserPendingFriendshipsAsync(string userId);
        
    }
}
