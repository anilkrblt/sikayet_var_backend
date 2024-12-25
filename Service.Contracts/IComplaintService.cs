using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IComplaintService
    {
        Task<IEnumerable<ComplaintDto>> GetAllComplaintsAsync(bool trackChanges);
        Task<ComplaintDto> GetComplaintByIdAsync(int complaintId, bool trackChanges);
        Task<IEnumerable<ComplaintDto>> GetComplaintsByUserAsync(int userId, bool trackChanges);
        Task<IEnumerable<ComplaintDto>> GetComplaintsByProductAsync(int productId, bool trackChanges);
        Task CreateComplaintAsync(ComplaintCreateDto complaint);
        void DeleteComplaint(int complaintId);
    }
}
