using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IChatService
    {
        public Task<Chat?> GetChatAsync(string id);
        public Task<List<Chat>> GetChatsForUserAsync(string userId);
        public Task CreateChatAsync(Chat chat);

        public Task CreatePrivateChatAsync(AppUser? first, AppUser? second);
        public Task UpdateChatAsync(Chat chat);
        public Task DeleteChatAsync(string chatId);
        public Task AddUserAsync(Chat chat, AppUser user);
        public Task<bool> RemoveUserAsync(Chat chat, AppUser user);
        public Task<List<AppUser>> GetAllUsersAsync(Chat chat);
        public Task<List<Chat>> SearchChatAsync(string userId, string query);

        public List<Chat> GetLast(string userdId, int count);
        public string GetFriendName(Chat chat, string currentUserId);

        public Task<bool> AnyAsync();

    }
}
