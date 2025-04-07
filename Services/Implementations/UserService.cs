

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly SocialDbContext _context;
        public UserService(SocialDbContext context)
        {
            _context = context;
        }
        public async Task<AppUser?> GetUserByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Media)
                .Include(u => u.Friendships)
                .Include(u => u.Chats)
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<AppUser?> GetUserWithCommunicationDetailsAsync(string userId)
        {
            return await _context.Users
               .Include(u => u.Chats)
               .Where(u => u.Id == userId)
               .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task CreateAsync(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<AppUser>> SearchUsersByName(string name, string currentId)
        {
            if(string.IsNullOrEmpty(name))
            {
                return Enumerable.Empty<AppUser>();
            }
            return await _context.Users.Where(u => u.UserName.Contains(name) && u.Id != currentId).ToListAsync();
        }
        public async Task<IEnumerable<AppUser>> GetAllUserFriendsAsync(string userId, List<Friendship> friends)
        {

            if (friends != null && friends.Any())
            {
                var friendIds = friends.Select(x => x.SenderId != userId ? x.SenderId : x.ReceiverId).ToList();

                return await _context.Users
                    .Where(u => friendIds.Contains(u.Id))
                    .ToListAsync();
            }

            return Enumerable.Empty<AppUser>();
        }
        public async Task<IEnumerable<AppUser>> GetAllConversationUsersAsync(string chatId)
        {
            return await _context.Users.Where(u => u.Chats.Any(c => c.Id == chatId)).ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> SearchUsersByNameAsync(string searchQuery)
        {
            return await _context.Users
                .Include(u => u.Media)
                .Where(u => u.FirstName.Contains(searchQuery) || u.LastName.Contains(searchQuery))
                .ToListAsync();
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.Users.AnyAsync();
        }

        public Task<List<AppUser>> GetAllAsync()
        {
            return _context.Users.ToListAsync();
        }
        public async Task AddProfilePictureAsync(AppUser user, Media media)
        {
            if (user != null && media!=null)
            {
                user.Media = media;
                user.MediaId = media.Id;
                _context.Update(user);
            }
            await _context.SaveChangesAsync();

        }


    }
}