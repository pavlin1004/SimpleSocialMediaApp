using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services
{
    public class FriendService : IFriendService
    {

        private readonly SocialDbContext _context;

        public FriendService(SocialDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Friendship f)
        {
            _context.Friendships.Add(f);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Friendship f)
        {
            _context.Friendships.Remove(f);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<string>> GetAllFriendsIdsAsync(string id)
        {
                return await _context.Friendships
                .Where(f => f.User1Id == id || f.User2Id == id)
                .Select(f => f.User1Id == id ? f.User2Id : f.User1Id) 
                .Distinct() 
                .ToListAsync(); 
        }
        public async Task<ICollection<AppUser>> GetAllFriends(string id)
        {
            var ids = await GetAllFriendsIdsAsync(id);

            return await _context.Users
            .Where(u => ids.Contains(u.Id)) 
            .ToListAsync();
        }

    }
}
