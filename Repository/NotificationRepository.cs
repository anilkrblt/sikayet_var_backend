using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(n => n.CreatedAt)
                .ToListAsync();

        public async Task<Notification> GetNotificationByIdAsync(int notificationId, bool trackChanges) =>
            await FindByCondition(n => n.Id == notificationId, trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId, bool trackChanges) =>
            await FindByCondition(n => n.UserId == userId, trackChanges).ToListAsync();

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsByUserAsync(int userId, bool trackChanges) =>
            await FindByCondition(n => (n.UserId == userId) && (bool)!n.IsRead, trackChanges).ToListAsync();

        public void CreateNotification(Notification notification) => Create(notification);
        public void DeleteNotification(Notification notification) => Delete(notification);
    }
}
