using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync(bool trackChanges);
        Task<NotificationDto> GetNotificationByIdAsync(int notificationId, bool trackChanges);
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserAsync(int userId, bool trackChanges);
        Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserAsync(int userId, bool trackChanges);
        Task CreateNotificationAsync(NotificationDto notification);
        Task MarkNotificationAsReadAsync(int notificationId);
        Task DeleteNotificationAsync(int notificationId);
    }
}
