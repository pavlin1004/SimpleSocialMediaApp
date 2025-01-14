using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Services.Implementations
{
    public class FriendshipsService : IFriendshipService
    {
        private readonly SocialDbContext _context;

        public FriendshipsService(SocialDbContext context)
        {
            _context = context;
        }

        public async Task<Friendship?> GetFriendshipById(string senderId, string receiverId)
        {
            return await CheckFriendship(senderId, receiverId);
        }

        public async Task<IEnumerable<Friendship>> GetUserAcceptedFriendshipsAsync(string userId)
        {
            return await _context.Friendships
                .Where(f => (f.SenderId == userId || f.ReceiverId == userId) && f.Type == FriendshipType.Accepted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Friendship>> GetUserPendingFriendshipsAsync(string userId)
        {

            return await _context.Friendships
                .Where(f => f.SenderId == userId || f.ReceiverId == userId && f.Type == FriendshipType.Pending)
                .ToListAsync();
        }
        public async Task SendFriendshipRequestAsync(string senderId, string receiverId)
        {
            if (CheckFriendship(senderId, receiverId) != null)
            {
                var friendship = new Friendship
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Type = FriendshipType.Pending

                };
                _context.Friendships.Add(friendship);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RemoveUserFriendshipsAsync(string senderId, string receiverId)
        {
            var friendship = await CheckFriendship(senderId, receiverId);
            if (friendship != null)
            {
                _context.RemoveRange(friendship);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AcceptUserFriendshipAsync(string senderId, string receiverId)
        {
            var friendship = await GetFriendshipById(senderId, receiverId);
            if (friendship != null)
            {

                friendship.Type = FriendshipType.Accepted;
                _context.Friendships.Update(friendship);
                await _context.SaveChangesAsync();
            }
        } 

        public async Task<Friendship?> CheckFriendship(string user1Id, string user2Id)
        {
            var friendship = await _context.Friendships
                .Where(f => (f.SenderId == user1Id && f.ReceiverId == user2Id) || (f.SenderId == user2Id && f.ReceiverId == user1Id))
                .FirstOrDefaultAsync();

            return friendship;
        }

     


    }
}
