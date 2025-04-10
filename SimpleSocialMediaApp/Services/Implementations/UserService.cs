

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

        public async Task<IEnumerable<AppUser>> SearchUsersByNameAsync(string searchQuery)
        {
            searchQuery = searchQuery.ToLower();
            return await _context.Users
                .Include(u => u.Media)
                .Where(u => u.FirstName.ToLower().Contains(searchQuery) || u.LastName.ToLower().Contains(searchQuery))
                .ToListAsync();
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.Users.AnyAsync();
        }

        public async Task<List<AppUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
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