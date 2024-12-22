using AutoMapper;
using Contracts;                       // ILoggerManager, IRepositoryManager vb.
using Entities.Models;                 // Comment entity
using Service.Contracts;               // ICommentService
using Shared.DataTransferObjects;      // CommentDto

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

        /// <summary>
        /// Tüm yorumların DTO listesini döner.
        /// </summary>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>CommentDto listesi</returns>
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

        /// <summary>
        /// Belirtilen Id'ye sahip yorumu DTO olarak döner.
        /// </summary>
        /// <param name="commentId">Aranacak yorumun ID'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>CommentDto veya null</returns>
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

        /// <summary>
        /// Bir şikâyete (complaintId) ait yorumları DTO listesi olarak döner.
        /// </summary>
        /// <param name="complaintId">Yorumları getirilecek şikâyet Id'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>CommentDto listesi</returns>
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

        /// <summary>
        /// Bir kullanıcı (userId) tarafından yazılan yorumları DTO listesi olarak döner.
        /// </summary>
        /// <param name="userId">Yorumları getirilecek kullanıcının Id'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>CommentDto listesi</returns>
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

        /// <summary>
        /// Yeni bir yorum (Comment) oluşturur.
        /// </summary>
        /// <param name="comment">Oluşturulacak yorumun DTO nesnesi</param>
        public async Task CreateCommentAsync(CommentDto comment)
        {
            if (comment == null)
            {
                _logger.LogError("CreateCommentAsync: CommentDto object is null.");
                return;
            }

            _logger.LogInfo($"Creating a new comment by UserId = {comment.UserId} for ComplaintId = {comment.ComplaintId}.");

            // DTO -> Entity
            var commentEntity = _mapper.Map<Comment>(comment);

            // Repository üzerinden ekle
            _repository.Comment.CreateComment(commentEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Comment created successfully with Id = {commentEntity.Id}.");
        }

        /// <summary>
        /// Belirtilen Id'ye sahip yorumu siler.
        /// </summary>
        /// <param name="commentId">Silinecek yorumun Id'si</param>
        public async Task DeleteCommentAsync(int commentId)
        {
            _logger.LogInfo($"Attempting to delete comment with Id = {commentId}.");

            // Silinecek yorumu getir
            var commentEntity = await _repository.Comment.GetCommentByIdAsync(commentId, trackChanges: false);
            if (commentEntity == null)
            {
                _logger.LogWarn($"Comment with Id = {commentId} not found. Deletion canceled.");
                return;
            }

            // Repository üzerinden sil
            _repository.Comment.DeleteComment(commentEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Comment with Id = {commentId} deleted successfully.");
        }
    }
}
