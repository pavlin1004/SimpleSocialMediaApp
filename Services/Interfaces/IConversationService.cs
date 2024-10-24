using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IConversationService
    {
        public Task<Conversation?> GetConversationAsync(string id);
        public Task<IEnumerable<Conversation>> GetConversationsForUserAsync(string userId);
        public Task CreateConversationAsync(Conversation conversation);
        public Task UpdateConversationAsync(Conversation conversation);
        public Task DeleteConversationAsync(string conversationId);
        public Task AddUserAsync(string conversationId, AppUser user);
        public Task RemoveUserAsync(string conversationId, AppUser user);

    }
}
