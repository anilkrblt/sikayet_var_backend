using Entities.Models;

namespace Contracts
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync(bool trackChanges);
        Task<Comment> GetCommentByIdAsync(int commentId, bool trackChanges);
        Task<IEnumerable<Comment>> GetCommentsByComplaintAsync(int complaintId, bool trackChanges);
        Task<IEnumerable<Comment>> GetCommentsByUserAsync(int userId, bool trackChanges);
        void CreateComment(Comment comment);
        void DeleteComment(Comment comment);
    }
}
