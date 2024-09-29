

using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services
{
    public class UserService 
    {
        private readonly SocialDbContext __dbContext;

        public UserService(SocialDbContext dbContext)
        {

            __dbContext = dbContext;
        }

        public async Task AddAsync(AppUser user)
        {
            this.__dbContext.Users.Add(user);
            await __dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string user)
        {
            var userToDelete = await __dbContext.Users.FindAsync(user);
            if(userToDelete!=null)
            {
                __dbContext.Users.Remove(userToDelete);
                await __dbContext.SaveChangesAsync();
            }
        }

        public async Task<AppUser> GetUserByIdAsync(string Id)
        {
            var user = await __dbContext.Users.FindAsync(Id);

            return user != null ? user : throw new KeyNotFoundException($"User with id {Id} not found!");                        
        }

        public async Task<AppUser> GetUserByNameAsync(string name)
        {
            var user = await __dbContext.Users.FindAsync(name);
            return user != null ? user : throw new Exception();
        }

      
    }
}