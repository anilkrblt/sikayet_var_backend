using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class CommentService : ICommentService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CommentService(IRepositoryManager repository,
                              ILoggerManager logger,
                              IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync(bool trackChanges)
        {
            _logger.LogInfo("Fetching all comments from the database.");

            var comments = await _repository.Comment.GetAllCommentsAsync(trackChanges);
            if (comments == null || !comments.Any())
            {
                _logger.LogWarn("No comments found in the database.");
                return Enumerable.Empty<CommentDto>();
            }

            // Entity -> DTO
            var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(comments);
            _logger.LogInfo($"{commentsDto.Count()} comment(s) fetched successfully.");

            return commentsDto;
        }

        public async Task<CommentDto> GetCommentByIdAsync(int commentId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching comment with Id = {commentId}.");

            var comment = await _repository.Comment.GetCommentByIdAsync(commentId, trackChanges);
            if (comment == null)
            {
                _logger.LogWarn($"Comment with Id = {commentId} not found.");
                return null;
            }

            var commentDto = _mapper.Map<CommentDto>(comment);
            _logger.LogInfo($"Comment with Id = {commentId} fetched successfully.");
            return commentDto;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByComplaintAsync(int complaintId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching comments for complaint with Id = {complaintId}.");

            var comments = await _repository.Comment.GetCommentsByComplaintAsync(complaintId, trackChanges);
            if (comments == null || !comments.Any())
            {
                _logger.LogWarn($"No comments found for complaint with Id = {complaintId}.");
                return Enumerable.Empty<CommentDto>();
            }

            var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(comments);
            _logger.LogInfo($"{commentsDto.Count()} comment(s) fetched for complaint with Id = {complaintId}.");

            return commentsDto;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByUserAsync(int userId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching comments for user with Id = {userId}.");

            var comments = await _repository.Comment.GetCommentsByUserAsync(userId, trackChanges);
            if (comments == null || !comments.Any())
            {
                _logger.LogWarn($"No comments found for user with Id = {userId}.");
                return Enumerable.Empty<CommentDto>();
            }

            var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(comments);
            _logger.LogInfo($"{commentsDto.Count()} comment(s) fetched for user with Id = {userId}.");

            return commentsDto;
        }


        public async Task<CommentDto> CreateCommentAsync(CommentCreateDto comment)
        {
            if (comment == null)
            {
                _logger.LogError("CreateCommentAsync: CommentDto object is null.");
                throw new ArgumentNullException();
            }

            _logger.LogInfo($"Creating a new comment by UserId = {comment.UserId} for ComplaintId = {comment.ComplaintId}.");

            // DTO -> Entity
            var commentEntity = _mapper.Map<Comment>(comment);
            commentEntity.CreatedAt = DateTime.Now;
            commentEntity.UpdatedAt = DateTime.Now;

            var commentDto = new CommentDto(commentEntity.Id, commentEntity.ComplaintId, commentEntity.UserId, commentEntity.Content, DateTime.Now, DateTime.Now);

            // Repository üzerinden ekle
            _repository.Comment.CreateComment(commentEntity);
            await _repository.SaveAsync();

            var notificationDto = new NotificationDto
            {
                UserId = (int)comment.UserId,
                Type = "CommentAdded",
                Content = $"Your complaint has a new comment: {comment.Content}",
                IsRead = false
            };
            var notification = _mapper.Map<Notification>(notificationDto);
            notification.CreatedAt = DateTime.Now;
            _repository.Notification.CreateNotification(notification);


            _logger.LogInfo($"Comment created successfully with Id = {commentEntity.Id}.");
            return commentDto;
        }



        public async Task DeleteCommentAsync(int commentId, int userId)
        {
            var comment = await _repository.Comment.GetCommentByIdAsync(commentId, trackChanges: true);
            if (comment is null)
            {
                _logger.LogWarn($"Comment with Id = {commentId} not found. Deletion canceled.");
                throw new KeyNotFoundException("Comment not found.");
            }

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("You are not authorized to delete this comment.");

            _repository.Comment.DeleteComment(comment);
            await _repository.SaveAsync();
            _logger.LogInfo($"Comment with Id = {commentId} deleted successfully.");

        }
        public async Task DeleteCommentByAdminAsync(int commentId)
        {
            var comment = await _repository.Comment.GetCommentByIdAsync(commentId, trackChanges: true);
            if (comment is null)
            {
                _logger.LogWarn($"Comment with Id = {commentId} not found. Deletion canceled.");
                throw new KeyNotFoundException("Comment not found.");
            }

            _repository.Comment.DeleteComment(comment);
            await _repository.SaveAsync();
        }


    }
}
