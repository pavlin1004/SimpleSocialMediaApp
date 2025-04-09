using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IUserService
    {
        public Task<AppUser?> GetUserByIdAsync(string userId);
        public Task<AppUser?> GetUserWithCommunicationDetailsAsync(string userId);        public Task CreateAsync(AppUser user);
        public Task UpdateAsync(AppUser user);
        public Task RemoveAsync(string id);
        public Task<IEnumerable<AppUser>> SearchUsersByNameAsync(string searchQuery);
        public Task<bool> AnyAsync();
        public Task AddProfilePictureAsync(AppUser user ,Media media);
        public Task<List<AppUser>> GetAllAsync();
    }
}
