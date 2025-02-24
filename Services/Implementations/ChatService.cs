using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Services.Implementations
{
    public class ChatService :IChatService
    {
        private readonly SocialDbContext _context;
        //private readonly IUserService _userService;
        public ChatService(SocialDbContext context)
        {
            _context = context;
            //_userService = userService;
        }
        public async Task<Chat?> GetConversationAsync(string conversationId)
        {
            return await _context.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == conversationId);
        }
        public async Task<IEnumerable<Chat>> GetConversationsForUserAsync(string userId)
        {
            return await _context.Chats
                .Where(c => c.Users.Any(u => u.Id == userId)) 
                .Include(c => c.Messages) 
                .ToListAsync();
        }

        public async Task<List<AppUser>> GetAllUsers(Chat chat)
        {
            return await _context.Users
                .Where(u => chat.Users.Select(x => x.Id).Contains(u.Id)) 
                .ToListAsync();
        }

        public async Task CreateConversationAsync(Chat conversation)
        {
           _context.Chats.Add(conversation);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateConversationAsync(Chat conversation)
        {
            _context.Chats.Update(conversation);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteConversationAsync(string conversationId)
        {
            var conversation = await GetConversationAsync(conversationId);
            if(conversation == null)
            {
                throw new NullReferenceException("Conversation doesn't exist in the database");
            }

            _context.Chats.Remove(conversation);
            await _context.SaveChangesAsync();

        }
        public async Task AddUserAsync(Chat chat, AppUser user)
        {
            chat.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> RemoveUserAsync(Chat chat, AppUser user)
        {
            if (!chat.Users.Any(u => u.Id == user.Id))
            {
                return false;
            }
            chat.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
