using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
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

        public async Task DeleteUserFriendshipsAsync(string userId)
        {
            var friendships = await _context.Friendships.Where(x => x.User1Id == userId || x.User2Id == userId).ToListAsync();
            if (friendships.Any())
            {
                _context.RemoveRange(friendships);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Friendship>> GetUserFriendshipsAsync(string userId)
        {
            return await _context.Friendships
                .Where(f => f.User1Id == userId || f.User2Id == userId)
                .ToListAsync();
        }


    }
}
