

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services
{
    public class UserService : IUserService
    {
        private readonly SocialDbContext _dbContext;

        public UserService(SocialDbContext dbContext)
        {

            _dbContext = dbContext;
        }

        public async Task UpdateAsync(AppUser user)
        {
            
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddAsync(AppUser user)
        {
            
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string user)
        {
            var userToDelete = await _dbContext.Users.FindAsync(user);
            if(userToDelete!=null)
            {
                _dbContext.Users.Remove(userToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<AppUser> GetUserByIdAsync(string Id)
        {
            var user = await _dbContext.Users.FindAsync(Id);

            return user != null ? user : throw new KeyNotFoundException($"User with id {Id} not found!");                        
        }

        public async Task<ICollection<AppUser>> GetUserByNameAsync(string name)
        {
            return await _dbContext.Users.Where(x => x.FirstName.Contains(name)|| x.LastName.Contains(name)).ToListAsync();
        }

        public async Task<ICollection<AppUser>> GetAllFriends(string id)
        {
           var friendships = await _dbContext.Friendships.
                Where(f => f.User1Id == id || f.User2Id == id).
                ToListAsync();

            var ids = friendships
                .Select(f => f.User1Id == id ? f.User2Id : f.User1Id)
                .Distinct()
                .ToList();

            return await FindFriendsByIds(ids);

        }

        private async Task<ICollection<AppUser>> FindFriendsByIds(ICollection<string> ids)
        {
            return await _dbContext.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
        }      
    }
}