using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Services.Implementations
{
    public class ConversationService :IConversationService
    {
        private readonly SocialDbContext _context;

        public ConversationService(SocialDbContext context)
        {
            _context = context;
        }
        public async Task<Conversation?> GetConversationAsync(string conversationId)
        {
            return await _context.Conversations
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == conversationId);
        }
        public async Task<IEnumerable<Conversation>> GetConversationsForUserAsync(string userId)
        {
            return await _context.Conversations
                .Where(c => c.Users.Any(u => u.Id == userId)) 
                .Include(c => c.Messages) 
                .ToListAsync();
        }
        public async Task CreateConversationAsync(Conversation conversation)
        {
           _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateConversationAsync(Conversation conversation)
        {
            _context.Conversations.Update(conversation);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteConversationAsync(string conversationId)
        {
            var conversation = await GetConversationAsync(conversationId);
            if(conversation == null)
            {
                throw new NullReferenceException("Conversation doesn't exist in the database");
            }

            _context.Conversations.Remove(conversation);
            await _context.SaveChangesAsync();

        }
        public async Task AddUserAsync(string conversationId, AppUser user)
        {
            var conversation = await GetConversationAsync(conversationId);
            if (conversation == null)
            {
                throw new NullReferenceException("Conversation doesn't exist in the database");
            }
            conversation.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveUserAsync(string conversationId, AppUser user)
        {

            var conversation = await GetConversationAsync(conversationId);
            if (conversation == null)
            {
                throw new NullReferenceException("Conversation doesn't exist in the database");
            }
            conversation.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
