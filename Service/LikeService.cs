using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class LikeService : ILikeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public LikeService(IRepositoryManager repository,
                           ILoggerManager logger,
                           IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<IEnumerable<LikeDto>> GetAllLikesAsync(bool trackChanges)
        {
            _logger.LogInfo("Fetching all likes from the database.");

            var likes = await _repository.Like.GetAllLikesAsync(trackChanges);
            if (likes == null || !likes.Any())
            {
                _logger.LogWarn("No likes found in the database.");
                return Enumerable.Empty<LikeDto>();
            }

            var likesDto = _mapper.Map<IEnumerable<LikeDto>>(likes);
            _logger.LogInfo($"{likesDto.Count()} like(s) fetched successfully.");
            return likesDto;
        }


        public async Task<LikeDto> GetLikeByIdAsync(int likeId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching like with Id = {likeId}.");

            var like = await _repository.Like.GetLikeByIdAsync(likeId, trackChanges);
            if (like == null)
            {
                _logger.LogWarn($"Like with Id = {likeId} not found.");
                return null;
            }

            var likeDto = _mapper.Map<LikeDto>(like);
            _logger.LogInfo($"Like with Id = {likeId} fetched successfully.");
            return likeDto;
        }


        public async Task<IEnumerable<LikeDto>> GetLikesByUserAsync(int userId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching likes for user with Id = {userId}.");

            var likes = await _repository.Like.GetLikesByUserAsync(userId, trackChanges);
            if (likes == null || !likes.Any())
            {
                _logger.LogWarn($"No likes found for user with Id = {userId}.");
                return Enumerable.Empty<LikeDto>();
            }

            var likesDto = _mapper.Map<IEnumerable<LikeDto>>(likes);
            _logger.LogInfo($"{likesDto.Count()} like(s) fetched for user with Id = {userId}.");
            return likesDto;
        }


        public async Task<IEnumerable<LikeDto>> GetLikesByComplaintAsync(int complaintId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching likes for complaint with Id = {complaintId}.");

            var likes = await _repository.Like.GetLikesByComplaintAsync(complaintId, trackChanges);
            if (likes == null || !likes.Any())
            {
                _logger.LogWarn($"No likes found for complaint with Id = {complaintId}.");
                return Enumerable.Empty<LikeDto>();
            }

            var likesDto = _mapper.Map<IEnumerable<LikeDto>>(likes);
            _logger.LogInfo($"{likesDto.Count()} like(s) fetched for complaint with Id = {complaintId}.");
            return likesDto;
        }

        public async Task CreateLikeAsync(LikeCreateDto like)
        {
            if (like == null)
            {
                _logger.LogError("CreateLikeAsync: LikeDto object is null.");
                return;
            }

            _logger.LogInfo($"Creating a new like from UserId = {like.UserId} for ComplaintId = {like.ComplaintId}.");

            var likeEntity = _mapper.Map<Like>(like);
            likeEntity.CreatedAt = DateTime.Now;


            _repository.Like.CreateLike(likeEntity);
            await _repository.SaveAsync();

            var user = await _repository.User.GetUserByIdAsync((int)like.UserId, false);

            var notificationDto = new NotificationDto
            {
                UserId = user.Id, 
                Type = "ComplaintLiked",
                Content = $"Your complaint has been liked by {user.Username}.",
                IsRead = false
            };
            var notification = _mapper.Map<Notification>(notificationDto);

            _repository.Notification.CreateNotification(notification);

            _logger.LogInfo($"Like created successfully with Id = {likeEntity.Id}.");
        }

        public async Task DeleteLikeAsync(int likeId)
        {
            _logger.LogInfo($"Attempting to delete like with Id = {likeId}.");

            var likeEntity = await _repository.Like.GetLikeByIdAsync(likeId, trackChanges: false);
            if (likeEntity == null)
            {
                _logger.LogWarn($"Like with Id = {likeId} not found. Deletion canceled.");
                return;
            }

            _repository.Like.DeleteLike(likeEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Like with Id = {likeId} deleted successfully.");
        }
    }
}
