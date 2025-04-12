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
        public async Task<List<Friendship>> GetUserAcceptedFriendshipsAsync(string userId)
        {
            return await _context.Friendships
                .Where(f => (f.SenderId == userId || f.ReceiverId == userId) && f.Type == FriendshipType.Accepted).Include(f => f.Sender).Include(f =>f.Receiver)
                .ToListAsync();
        }

        // Get all pending friendships for a user
        public async Task<List<Friendship>> GetUserPendingFriendshipsAsync(string userId)
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
                _context.Friendships.Remove(friendship); 
                await _context.SaveChangesAsync(); 
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
            //All Senders & receivers + Media (profile pictures)
            var friendships = await _context.Friendships
             .Where(f => f.SenderId == userId || f.ReceiverId == userId)
             .Where(f => f.Type == FriendshipType.Accepted) 
             .Include(f => f.Sender) 
                 .ThenInclude(u => u.Media) 
             .Include(f => f.Receiver) 
                 .ThenInclude(u => u.Media) 
             .ToListAsync();


            var users = friendships
                .Select(f => f.SenderId == userId ? f.Receiver : f.Sender)
                .ToList();

            return users;

        }

        public async Task<List<string>> GetAllFriendsIds(string userId)
        {
            var friends = await GetAllFriends(userId);
            return friends.Select(f => f.Id).ToList();
        }

        public async Task<(List<AppUser>,List<AppUser>)> GetNonFriendUsers(string userId)
        {
            var friendIds = await _context.Friendships
                 .Where(f => (f.SenderId == userId || f.ReceiverId == userId) && f.Type == FriendshipType.Accepted)
                 .Select(f => f.SenderId == userId ? f.ReceiverId : f.SenderId)
                 .ToListAsync();

            // Get IDs of Pending Friend Requests
            var pendingIds = await _context.Friendships
                .Where(f => (f.SenderId == userId || f.ReceiverId == userId) && f.Type == FriendshipType.Pending)
                .Select(f => f.SenderId == userId ? f.ReceiverId : f.SenderId)
                .ToListAsync();

            // Fetch Pending Users with Media
            var pendingUsers = await _context.Users
                .Include(u => u.Media)
                .Where(u => pendingIds.Contains(u.Id))
                .ToListAsync();

            // Fetch Non-Friends (Exclude Current User, Friends, and Pending Users)
            var nonFriends = await _context.Users
                .Include(u => u.Media)
                .Where(u => u.Id != userId && !friendIds.Contains(u.Id) && !pendingIds.Contains(u.Id))
                .ToListAsync();

            return (pendingUsers, nonFriends);
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
