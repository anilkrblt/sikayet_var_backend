using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public NotificationsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _serviceManager.NotificationService.GetAllNotificationsAsync(trackChanges: false);
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _serviceManager.NotificationService.GetNotificationByIdAsync(id, trackChanges: false);
            if (notification is null)
                return NotFound();

            return Ok(notification);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetNotificationsByUser(int userId)
        {
            var notifications = await _serviceManager.NotificationService.GetNotificationsByUserAsync(userId, trackChanges: false);
            return Ok(notifications);
        }

        [HttpGet("user/{userId}/unread")]
        public async Task<IActionResult> GetUnreadNotificationsByUser(int userId)
        {
            var unreadNotifications = await _serviceManager.NotificationService.GetUnreadNotificationsByUserAsync(userId, trackChanges: false);
            return Ok(unreadNotifications);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            await _serviceManager.NotificationService.DeleteNotificationAsync(id);
            return NoContent();
        }
  
    }
}
