

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly SocialDbContext _context;
        private readonly IMediaService _media;
        private readonly IFriendshipService _friendships;

        public UserService(SocialDbContext context, IMediaService media, IFriendshipService friendships)
        {
            _context = context;
            _media = media;
            _friendships = friendships;
        }

        public async Task<AppUser?> GetUserByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Media)
                .Include(u => u.Friendships)
                .Include(u => u.Conversations)
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                await _media.DeleteMediaUserAsync(id);
                await _friendships.DeleteUserFriendshipsAsync(id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<AppUser>> GetAllUserFriendsAsync(string userId)
        {
            var friendships = await _friendships.GetUserFriendshipsAsync(userId);

            if (friendships != null && friendships.Any())
            {
                
                var friendIds = friendships.Select(x => x.User1Id != userId ? x.User1Id : x.User2Id).ToList();

                return await _context.Users
                    .Where(u => friendIds.Contains(u.Id))
                    .ToListAsync();
            }

            return new List<AppUser>();
        }
    }
}