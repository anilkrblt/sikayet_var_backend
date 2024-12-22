using AutoMapper;
using Contracts;                   // ILoggerManager, IRepositoryManager vb.
using Entities.Models;             // Like entity
using Service.Contracts;           // ILikeService
using Shared.DataTransferObjects;   // LikeDto

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

        /// <summary>
        /// Tüm "beğeni"lerin (Like) DTO listesini döner.
        /// </summary>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>LikeDto listesi</returns>
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

        /// <summary>
        /// Belirtilen Id'ye sahip "beğeni"yi (Like) DTO olarak döner.
        /// </summary>
        /// <param name="likeId">Aranacak beğeninin ID'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>LikeDto veya null</returns>
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

        /// <summary>
        /// Belirli bir kullanıcıya (userId) ait beğenileri DTO listesi olarak döner.
        /// </summary>
        /// <param name="userId">Beğenileri getirilecek kullanıcının Id'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>LikeDto listesi</returns>
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

        /// <summary>
        /// Belirli bir şikâyete (complaintId) ait beğenileri DTO listesi olarak döner.
        /// </summary>
        /// <param name="complaintId">Beğenileri getirilecek şikâyetin Id'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>LikeDto listesi</returns>
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

        /// <summary>
        /// Yeni bir beğeni (Like) oluşturur.
        /// </summary>
        /// <param name="like">Oluşturulacak beğeninin DTO nesnesi</param>
        public async Task CreateLikeAsync(LikeDto like)
        {
            if (like == null)
            {
                _logger.LogError("CreateLikeAsync: LikeDto object is null.");
                return;
            }

            _logger.LogInfo($"Creating a new like from UserId = {like.UserId} for ComplaintId = {like.ComplaintId}.");

            var likeEntity = _mapper.Map<Like>(like);

            _repository.Like.CreateLike(likeEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Like created successfully with Id = {likeEntity.Id}.");
        }

        /// <summary>
        /// Belirtilen Id'ye sahip beğeniyi siler.
        /// </summary>
        /// <param name="likeId">Silinecek beğeninin Id'si</param>
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
