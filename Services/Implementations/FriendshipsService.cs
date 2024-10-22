using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Services.Implementations
{
    public class FriendshipsService : IFriendshipService
    {
        private readonly SocialDbContext _context;

        public FriendshipsService(SocialDbContext context)
        {
            _context = context;
        }

        public async Task<Friendship?> GetFriendshipById(string Id)
        {
            return await _context.Friendships.FindAsync(Id); 
        }

        public async Task<IEnumerable<Friendship>> GetUserAcceptedFriendshipsAsync(string userId)
        {
            return await _context.Friendships
                .Where(f => f.SenderId == userId || f.ReceiverId == userId && f.Type == FriendshipType.Accepted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Friendship>> GetUserPendingFriendshipsAsync(string userId)
        {

            return await _context.Friendships
                .Where(f => f.SenderId == userId || f.ReceiverId == userId && f.Type == FriendshipType.Pending)
                .ToListAsync();
        }
        public async Task CreateFriendshipRequestAsync(Friendship f)
        {
            _context.Friendships.Add(f);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> RejectUserFriendshipsAsync(string userId)
        {
            var friendships = await _context.Friendships.Where(x => x.SenderId == userId || x.ReceiverId == userId).ToListAsync();
            if (friendships.Any())
            {
                _context.RemoveRange(friendships);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AcceptUserFriendshipAsync(string friendshipid)
        {
            var friendship = await GetFriendshipById(friendshipid);
            if (friendship == null)
            {
                return false;
            }

            friendship.Type = FriendshipType.Accepted;

            _context.Friendships.Update(friendship);
            await _context.SaveChangesAsync();

            return true;
        }

        


    }
}
