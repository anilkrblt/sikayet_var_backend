using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetAllCommentsAsync(bool trackChanges);
        Task<CommentDto> GetCommentByIdAsync(int commentId, bool trackChanges);
        Task<IEnumerable<CommentDto>> GetCommentsByComplaintAsync(int complaintId, bool trackChanges);
        Task<IEnumerable<CommentDto>> GetCommentsByUserAsync(int userId, bool trackChanges);
        Task CreateCommentAsync(CommentDto comment);
        Task DeleteCommentAsync(int commentId);
    }
}