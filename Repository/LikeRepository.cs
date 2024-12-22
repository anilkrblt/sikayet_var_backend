using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class LikeRepository : RepositoryBase<Like>, ILikeRepository
    {
        public LikeRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Like>> GetAllLikesAsync(bool trackChanges) =>
            await FindAll(trackChanges).OrderBy(l => l.Id).ToListAsync();

        public async Task<Like> GetLikeByIdAsync(int likeId, bool trackChanges) =>
            await FindByCondition(l => l.Id == likeId, trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Like>> GetLikesByUserAsync(int userId, bool trackChanges) =>
            await FindByCondition(l => l.UserId == userId, trackChanges).ToListAsync();

        public async Task<IEnumerable<Like>> GetLikesByComplaintAsync(int complaintId, bool trackChanges) =>
            await FindByCondition(l => l.ComplaintId == complaintId, trackChanges).ToListAsync();

        public void CreateLike(Like like) => Create(like);
        public void DeleteLike(Like like) => Delete(like);
    }
}
