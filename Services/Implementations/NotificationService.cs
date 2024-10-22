using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly SocialDbContext _context;

        public NotificationService(SocialDbContext context)
        {
            this._context = context;
        }
        public async Task<Notification?> GetNotificationById(string Id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<ICollection<Notification>> GetAllUserNotifications(string UserId)
        {
            return await _context.Notifications.Where(n => n.UserToId == UserId).ToListAsync();
        }

        public async Task AddNotification(Notification n)
        {
            _context.Notifications.Add(n);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveNotification(string Id)
        {
            var n = await GetNotificationById(Id);
            if (n != null)
            {
                _context.Notifications.Remove(n);
                await _context.SaveChangesAsync();
            }
        }
    }
}
