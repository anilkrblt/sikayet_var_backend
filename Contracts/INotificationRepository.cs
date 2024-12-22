using Entities.Models;

namespace Contracts
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync(bool trackChanges);
        Task<Notification> GetNotificationByIdAsync(int notificationId, bool trackChanges);
        Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId, bool trackChanges);
        Task<IEnumerable<Notification>> GetUnreadNotificationsByUserAsync(int userId, bool trackChanges);
        void CreateNotification(Notification notification);
        void DeleteNotification(Notification notification);
    }
}
