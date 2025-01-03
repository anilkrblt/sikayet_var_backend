using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class NotificationService : INotificationService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public NotificationService(IRepositoryManager repository,
                                   ILoggerManager logger,
                                   IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync(bool trackChanges)
        {
            _logger.LogInfo("Fetching all notifications from the database.");

            var notifications = await _repository.Notification.GetAllNotificationsAsync(trackChanges);
            if (notifications == null || !notifications.Any())
            {
                _logger.LogWarn("No notifications found in the database.");
                return Enumerable.Empty<NotificationDto>();
            }

            // Entity -> DTO
            var notificationsDto = _mapper.Map<IEnumerable<NotificationDto>>(notifications);
            _logger.LogInfo($"{notificationsDto.Count()} notification(s) fetched successfully.");

            return notificationsDto;
        }


        public async Task<NotificationDto> GetNotificationByIdAsync(int notificationId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching notification with Id = {notificationId}.");

            var notification = await _repository.Notification.GetNotificationByIdAsync(notificationId, trackChanges);
            if (notification == null)
            {
                _logger.LogWarn($"Notification with Id = {notificationId} not found.");
                return null;
            }

            var notificationDto = _mapper.Map<NotificationDto>(notification);
            _logger.LogInfo($"Notification with Id = {notificationId} fetched successfully.");
            return notificationDto;
        }


        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserAsync(int userId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching notifications for user with Id = {userId}.");

            var notifications = await _repository.Notification.GetNotificationsByUserAsync(userId, trackChanges);
            if (notifications == null || !notifications.Any())
            {
                _logger.LogWarn($"No notifications found for user with Id = {userId}.");
                return Enumerable.Empty<NotificationDto>();
            }

            var notificationsDto = _mapper.Map<IEnumerable<NotificationDto>>(notifications);
            _logger.LogInfo($"{notificationsDto.Count()} notification(s) fetched for user with Id = {userId}.");

            return notificationsDto;
        }


        public async Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserAsync(int userId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching unread notifications for user with Id = {userId}.");

            var unreadNotifications = await _repository.Notification.GetUnreadNotificationsByUserAsync(userId, trackChanges);
            if (unreadNotifications == null || !unreadNotifications.Any())
            {
                _logger.LogWarn($"No unread notifications found for user with Id = {userId}.");
                return Enumerable.Empty<NotificationDto>();
            }

            var unreadNotificationsDto = _mapper.Map<IEnumerable<NotificationDto>>(unreadNotifications);
            _logger.LogInfo($"{unreadNotificationsDto.Count()} unread notification(s) fetched for user with Id = {userId}.");

            return unreadNotificationsDto;
        }


        public async Task CreateNotificationAsync(NotificationDto notification)
        {
            if (notification == null)
            {
                _logger.LogError("CreateNotificationAsync: NotificationDto object is null.");
                return;
            }

            _logger.LogInfo($"Creating a new notification for user with Id = {notification.UserId}.");

            // DTO -> Entity
            var notificationEntity = _mapper.Map<Notification>(notification);

            // Repository üzerinden ekle
            _repository.Notification.CreateNotification(notificationEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Notification created successfully with Id = {notificationEntity.Id}.");
        }


        public async Task MarkNotificationAsReadAsync(int notificationId)
        {
            _logger.LogInfo($"Attempting to mark notification with Id = {notificationId} as read.");

            // 1) Bildirimi bul
            var notificationEntity = await _repository.Notification.GetNotificationByIdAsync(notificationId, trackChanges: true);
            if (notificationEntity == null)
            {
                _logger.LogWarn($"Notification with Id = {notificationId} not found. Mark-as-read canceled.");
                return;
            }

            // 2) Okundu olarak işaretle
            notificationEntity.IsRead = true;

            // 3) Güncellenmiş bildirimi repository'ye bildir

            // 4) Kaydet
            await _repository.SaveAsync();

            _logger.LogInfo($"Notification with Id = {notificationId} was marked as read successfully.");
        }


        public async Task DeleteNotificationAsync(int notificationId)
        {
            _logger.LogInfo($"Attempting to delete notification with Id = {notificationId}.");

            var notificationEntity = await _repository.Notification.GetNotificationByIdAsync(notificationId, trackChanges: false);
            if (notificationEntity == null)
            {
                _logger.LogWarn($"Notification with Id = {notificationId} not found. Deletion canceled.");
                return;
            }

            _repository.Notification.DeleteNotification(notificationEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Notification with Id = {notificationId} deleted successfully.");
        }

        public async Task SendNotificationAsync(NotificationDto notificationDto)
        {
            if (notificationDto == null)
            {
                _logger.LogError("SendNotificationAsync: NotificationDto object is null.");
                throw new ArgumentNullException(nameof(notificationDto));
            }

            _logger.LogInfo($"Sending notification to user with Id = {notificationDto.UserId}.");

            // DTO -> Entity
            var notificationEntity = _mapper.Map<Notification>(notificationDto);

            // Set created date
            notificationEntity.CreatedAt = DateTime.UtcNow;

            // Add the notification to the repository
            _repository.Notification.CreateNotification(notificationEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Notification sent successfully and saved to the database with Id = {notificationEntity.Id}.");
        }

    }
}
