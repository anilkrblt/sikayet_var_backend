using Entities.Models;

namespace Contracts
{
    public interface ILikeRepository
    {
        Task<IEnumerable<Like>> GetAllLikesAsync(bool trackChanges);
        Task<Like> GetLikeByIdAsync(int likeId, bool trackChanges);
        Task<IEnumerable<Like>> GetLikesByUserAsync(int userId, bool trackChanges);
        Task<IEnumerable<Like>> GetLikesByComplaintAsync(int complaintId, bool trackChanges);
        void CreateLike(Like like);
        void DeleteLike(Like like);
    }
}
