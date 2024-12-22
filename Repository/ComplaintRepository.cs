using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ComplaintRepository : RepositoryBase<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Complaint>> GetAllComplaintsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

        public async Task<Complaint> GetComplaintByIdAsync(int complaintId, bool trackChanges) =>
            await FindByCondition(c => c.Id == complaintId, trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Complaint>> GetComplaintsByUserAsync(int userId, bool trackChanges) =>
            await FindByCondition(c => c.UserId == userId, trackChanges).ToListAsync();

        public async Task<IEnumerable<Complaint>> GetComplaintsByProductAsync(int productId, bool trackChanges) =>
            await FindByCondition(c => c.ProductId == productId, trackChanges).ToListAsync();

        public void CreateComplaint(Complaint complaint) => Create(complaint);
        public void DeleteComplaint(Complaint complaint) => Delete(complaint);
    }
}
