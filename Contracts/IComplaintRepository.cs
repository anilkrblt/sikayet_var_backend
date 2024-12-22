using Entities.Models;

namespace Contracts
{
    public interface IComplaintRepository
    {
        Task<IEnumerable<Complaint>> GetAllComplaintsAsync(bool trackChanges);
        Task<Complaint> GetComplaintByIdAsync(int complaintId, bool trackChanges);
        Task<IEnumerable<Complaint>> GetComplaintsByUserAsync(int userId, bool trackChanges);
        Task<IEnumerable<Complaint>> GetComplaintsByProductAsync(int productId, bool trackChanges);
        void CreateComplaint(Complaint complaint);
        void DeleteComplaint(Complaint complaint);
    }
}
