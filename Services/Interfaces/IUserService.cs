using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IUserService
    {
        public Task<AppUser?> GetUserByIdAsync(string userId);

        public Task<AppUser?> GetUserWithCommunicationDetailsAsync(string userId);
        public Task<IEnumerable<AppUser>> GetAllUserFriendsAsync(string userId);
        public Task CreateAsync(AppUser user);
        public Task UpdateAsync(AppUser user);
        public Task RemoveAsync(string id);
        public Task<IEnumerable<AppUser>> SearchUsersByNameAsync(string searchQuery);
        public Task<IEnumerable<AppUser>> GetAllConversationUsersAsync(string userId);
        public Task<bool> AnyAsync();

        public Task<List<AppUser>> GetAllAsync();

    }
}
