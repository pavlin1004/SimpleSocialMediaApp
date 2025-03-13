using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;
using System.Net.WebSockets;

namespace SimpleSocialApp.Services.Implementations
{
    public class FriendshipsService : IFriendshipService
    {
        private readonly SocialDbContext _context;

        public FriendshipsService(SocialDbContext context)
        {
            _context = context;
        }

        // Get the existing friendship between two users, if any.
        public async Task<Friendship?> GetFriendshipById(string senderId, string receiverId)
        {
            return await CheckFriendship(senderId, receiverId);
        }

        // Get all accepted friendships for a user
        public async Task<IEnumerable<Friendship>> GetUserAcceptedFriendshipsAsync(string userId)
        {
            return await _context.Friendships
                .Where(f => (f.SenderId == userId || f.ReceiverId == userId) && f.Type == FriendshipType.Accepted).Include(f => f.Sender).Include(f =>f.Receiver)
                .ToListAsync();
        }

        // Get all pending friendships for a user
        public async Task<IEnumerable<Friendship>> GetUserPendingFriendshipsAsync(string userId)
        {
            return await _context.Friendships
                .Where(f => (f.SenderId == userId || f.ReceiverId == userId) && f.Type == FriendshipType.Pending)
                .ToListAsync();
        }

        // Send a friendship request. If no existing friendship, create a new one.
        public async Task SendFriendshipRequestAsync(string senderId, string receiverId)
        {
            var existingFriendship = await CheckFriendship(senderId, receiverId);

            if (existingFriendship == null) // Only create a friendship if it does not already exist
            {
                var friendship = new Friendship
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Type = FriendshipType.Pending
                };

                _context.Friendships.Add(friendship);
                await _context.SaveChangesAsync(); // Use await to prevent concurrency issues
            }
        }

        // Remove a friendship between sender and receiver
        public async Task RemoveUserFriendshipsAsync(string senderId, string receiverId)
        {
            var friendship = await CheckFriendship(senderId, receiverId);

            if (friendship != null)
            {
                _context.Friendships.Remove(friendship); // Use Remove instead of RemoveRange for a single entity
                await _context.SaveChangesAsync(); // Use await here as well to avoid concurrency issues
            }
        }

        // Accept a friendship request
        public async Task AcceptUserFriendshipAsync(string senderId, string receiverId)
        {
            var friendship = await GetFriendshipById(senderId, receiverId);

            if (friendship != null)
            {
                friendship.Type = FriendshipType.Accepted;
                _context.Friendships.Update(friendship); // Update the existing record

                try
                {
                    await _context.SaveChangesAsync(); // Save changes and handle concurrency errors
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency exception here (e.g., reload data)
                    throw new Exception("The friendship was modified by another user, please reload the page.");
                }
            }
        }

        // Check if a friendship exists between two users
        public async Task<Friendship?> CheckFriendship(string user1Id, string user2Id)
        {
            return await _context.Friendships
                .Where(f => (f.SenderId == user1Id && f.ReceiverId == user2Id) ||
                            (f.SenderId == user2Id && f.ReceiverId == user1Id))
                .FirstOrDefaultAsync();
        }

        // Get all friends for a user based on accepted friendships
        public async Task<List<AppUser>> GetAllFriends(string userId)
        {
            var friendships = await GetUserAcceptedFriendshipsAsync(userId);
            return friendships.Select(u => u.SenderId == userId ? u.Receiver : u.Sender).ToList();
        }

        public async Task<List<string>> GetAllFriendsIds(string userId)
        {
            var friends = await GetAllFriends(userId);
            return friends.Select(f => f.Id).ToList();
        }

        public async Task<List<AppUser>> GetNonFriendUsers(string userId)
        {
            var friends = await _context.Friendships
                .Where(f => f.SenderId == userId || f.ReceiverId == userId)  // Get friendships where the user is involved
                .Select(f => f.SenderId == userId ? f.ReceiverId : f.SenderId) // Select the friend's ID
                .ToListAsync();

            var nonFriends = await _context.Users
                .Where(u => u.Id != userId && !friends.Contains(u.Id))  // Exclude current user & their friends
                .ToListAsync();

            return nonFriends;
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.Friendships.AnyAsync();
        }
        public async Task CreateAsync(Friendship f)
        {
            if(f!=null)
            {
                await _context.Friendships.AddAsync(f);
                await _context.SaveChangesAsync();
            }
        }


    }
}
