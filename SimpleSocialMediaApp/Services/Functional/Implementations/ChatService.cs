using CloudinaryDotNet.Actions;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSociaMedialApp.Services.Functional.Interfaces;
using System.Security.Claims;

namespace SimpleSociaMedialApp.Services.Functional.Implementations
{
    public class ChatService : IChatService
    {
        private readonly SocialDbContext _context;
        public ChatService(SocialDbContext context)
        {
            _context = context;
        }
        public async Task<Chat?> GetChatAsync(string conversationId)
        {
            var chat = await _context.Chats
                .Include(c => c.Users)
                .ThenInclude(u => u.Media)
                .Include(c => c.Messages)
                .Where(c => c.Id == conversationId)
                .FirstOrDefaultAsync();

            if (chat != null && chat.Messages != null)
            {
                // Order messages by CreatedDateTime after fetching
                chat.Messages = chat.Messages.OrderByDescending(m => m.CreatedDateTime).Reverse().ToList();
            }

            return chat;
        }
        public async Task<List<Chat>> GetChatsForUserAsync(string userId)
        {
            return await _context.Chats
                .Where(c => c.Users.Any(u => u.Id == userId))
                .Include(c => c.Messages)
                .Include(c => c.Users)
                .ToListAsync();
        }

        public async Task<List<AppUser>> GetAllUsersAsync(Chat chat)
        {
            return await _context.Users
                .Where(u => chat.Users.Select(x => x.Id).Contains(u.Id))
                .ToListAsync();
        }

        public async Task CreateChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task CreatePrivateChatAsync(AppUser? first, AppUser? second)
        {
            if (first != null && second != null)
            {

                var chat = new Chat
                {
                    CreatedDateTime = DateTime.UtcNow,
                    Type = ChatType.Private,
                    Users = new List<AppUser> { first, second }
                };
                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteChatAsync(string chatId)
        {
            var chat = await GetChatAsync(chatId);
            if (chat == null)
            {
                throw new NullReferenceException("Conversation doesn't exist in the database");
            }

            _context.Chats.Remove(chat);
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

        public string GetFriendName(Chat chat, string currentUserId)
        {
            var friend = chat.Users.Where(u => u.Id != currentUserId).FirstOrDefault();
            return string.Concat(friend.FirstName + " " + friend.LastName).ToString();
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.Chats.AnyAsync();
        }

        public async Task<List<Chat>> SearchChatAsync(string userId, string query)
        {
            query = query.ToLower(); // Make search case-insensitive

            var chats = _context.Chats.Include(c => c.Users);

            var groupChats = await chats
                .Where(c => c.Type == ChatType.Group && c.Title.ToLower().Contains(query) && c.Users.Any(u => u.Id == userId))
                .ToListAsync();

            var privateChats = await chats
                .Where(c => c.Type == ChatType.Private && c.Users.Any(u => u.Id == userId))
                .ToListAsync();

            // Check if FirstName + LastName contains query
            var filteredPrivateChats = privateChats
                .Where(c =>
                {
                    var otherUser = c.Users.FirstOrDefault(u => u.Id != userId);
                    return otherUser != null && $"{otherUser.FirstName} {otherUser.LastName}".ToString().ToLower().Contains(query);
                })
                .ToList();

            return filteredPrivateChats.Union(groupChats).ToList();
        }

        public async Task<List<Chat>> OrderChatsByLastMessages(string userId, int count)
        {
            // Retrieve chats that have messages and include the messages
            var chats = await _context.Chats
                .Where(c => c.Users.Any(u => u.Id == userId))
                .Include(c => c.Messages)
                .Include(c => c.Users)
                .ToListAsync();

            // Filter chats that have messages and order by the latest message in each chat
            var orderedChats = chats
                .Where(c => c.Messages != null && c.Messages.Any())
                .OrderByDescending(c => c.Messages?
                    .OrderBy(m => m.CreatedDateTime)
                    .LastOrDefault()?.CreatedDateTime)
                .Take(count)
                .ToList();

            return orderedChats;
        }
    }
}
