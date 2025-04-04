using CloudinaryDotNet.Actions;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;
using System.Security.Claims;

namespace SimpleSocialApp.Services.Implementations
{
    public class ChatService : IChatService
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
            var chat = await _context.Chats
                .Include(c => c.Users)
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
        public async Task<List<Chat>> GetConversationsForUserAsync(string userId)
        {
            return await _context.Chats
                .Where(c => c.Users.Any(u => u.Id == userId))
                .Include(c => c.Messages)
                .Include(c => c.Users)
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

        public async Task UpdateConversationAsync(Chat conversation)
        {
            _context.Chats.Update(conversation);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteConversationAsync(string conversationId)
        {
            var conversation = await GetConversationAsync(conversationId);
            if (conversation == null)
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

        public string GetFriendName(Chat chat, string currentUserId)
        {
            var friend = chat.Users.Where(u => u.Id != currentUserId).FirstOrDefault();
            return string.Concat(friend.FirstName + " " + friend.LastName).ToString();
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.Chats.AnyAsync();
        }

        public async Task<List<Chat>> SearchChat(string userId, string query)
        {
            query = query.ToLower(); // Make search case-insensitive

            var chats = _context.Chats.Include(c => c.Users);
            // Search Group Chats
            var groupChats = await chats
                .Where(c => c.Type == ChatType.Group && c.Title.ToLower().Contains(query) && c.Users.Any(u=>u.Id == userId))
                .ToListAsync();

            // Search Private Chats
            var privateChats = await chats
                .Where(c => c.Type == ChatType.Private && c.Users.Any(u => u.Id == userId))
                .ToListAsync();

            // Filter Private Chats by the *other* user's full name
            var filteredPrivateChats = privateChats
                .Where(c =>
                {
                    var otherUser = c.Users.FirstOrDefault(u => u.Id != userId); // Get the other user
                    return otherUser != null && ($"{otherUser.FirstName} {otherUser.LastName}").ToString().ToLower().Contains(query);
                })
                .ToList();

            // Combine results
            return filteredPrivateChats.Union(groupChats).ToList();
        }

        public List<Chat> GetLast(string userId, int count)
        {
            // Retrieve chats that have messages and include the messages
            var chats = _context.Chats
                .Where(c => c.Users.Any(u => u.Id == userId)) // Ensure the chat contains the specified user
                .Include(c => c.Messages)
                .Include(c=> c.Users)// Include related messages
                .ToList(); // Fetch all relevant chats (you can add pagination or other filtering if needed)

            // Filter chats that have messages and order by the latest message in each chat
            var orderedChats = chats
                .Where(c => c.Messages != null && c.Messages.Any()) // Only include chats that have messages
                .OrderByDescending(c => c.Messages?
                    .OrderBy(m => m.CreatedDateTime) // Order messages by CreatedDateTime in ascending order
                    .LastOrDefault()?.CreatedDateTime) // Get the latest message by CreatedDateTime
                .Take(count) // Take the specified number of chats
                .ToList();

            return orderedChats;
        }
    }
}
