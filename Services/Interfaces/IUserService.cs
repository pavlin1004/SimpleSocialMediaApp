using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IUserService
    {
        public Task<AppUser?> GetUserByIdAsync(string id);

        public Task<IEnumerable<AppUser>> GetAllUserFriendsAsync(string userId);

        public Task AddAsync(AppUser user);

        public Task UpdateAsync(AppUser user);

        public Task DeleteAsync(string id);

    }
}
