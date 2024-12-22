using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(c => c.Id)
                .ToListAsync();

        public async Task<Comment> GetCommentByIdAsync(int commentId, bool trackChanges) =>
            await FindByCondition(c => c.Id == commentId, trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Comment>> GetCommentsByComplaintAsync(int complaintId, bool trackChanges) =>
            await FindByCondition(c => c.ComplaintId == complaintId, trackChanges).ToListAsync();

        public async Task<IEnumerable<Comment>> GetCommentsByUserAsync(int userId, bool trackChanges) =>
            await FindByCondition(c => c.UserId == userId, trackChanges).ToListAsync();

        public void CreateComment(Comment comment) => Create(comment);
        public void DeleteComment(Comment comment) => Delete(comment);
    }
}
