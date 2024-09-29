using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services
{
    public interface IUserService
    {

        public Task<AppUser> GetUserByIdAsync(string Id);
        public Task<ICollection<AppUser>> GetUserByNameAsync(string name);

        public Task AddAsync(AppUser user);

        public Task UpdateAsync(AppUser user);

        public Task DeleteAsync(string user);
        
    }
}
