using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ILikeService
    {
        Task<IEnumerable<LikeDto>> GetAllLikesAsync(bool trackChanges);
        Task<LikeDto> GetLikeByIdAsync(int likeId, bool trackChanges);
        Task<IEnumerable<LikeDto>> GetLikesByUserAsync(int userId, bool trackChanges);
        Task<IEnumerable<LikeDto>> GetLikesByComplaintAsync(int complaintId, bool trackChanges);
        Task CreateLikeAsync(LikeDto like);
        Task DeleteLikeAsync(int likeId);
    }
}
