using SimpleSocialApp.Data.Models;

namespace SimpleSociaMedialApp.Services.Functional.Interfaces
{
    public interface INotificationService
    {
        public Task<Notification?> GetNotificationById(string Id);
        public Task<ICollection<Notification>> GetAllUserNotifications(string UserId);
        public Task AddNotification(Notification n);
        public Task RemoveNotification(string Id);


    }
}
