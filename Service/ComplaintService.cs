using AutoMapper;
using Contracts;                  
using Entities.Models;           
using Service.Contracts;           
using Shared.DataTransferObjects; 
namespace Service
{
    public class ComplaintService : IComplaintService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ComplaintService(IRepositoryManager repository,
                                ILoggerManager logger,
                                IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<IEnumerable<ComplaintDto>> GetAllComplaintsAsync(bool trackChanges)
        {
            _logger.LogInfo("Fetching all complaints from the database.");

            var complaints = await _repository.Complaint.GetAllComplaintsAsync(trackChanges);
            if (complaints == null || !complaints.Any())
            {
                _logger.LogWarn("No complaints found in the database.");
                return Enumerable.Empty<ComplaintDto>();
            }

            var complaintsDto = _mapper.Map<IEnumerable<ComplaintDto>>(complaints);
            _logger.LogInfo($"{complaintsDto.Count()} complaint(s) fetched successfully.");
            return complaintsDto;
        }


        public async Task<ComplaintDto> GetComplaintByIdAsync(int complaintId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching complaint with Id = {complaintId}.");

            var complaint = await _repository.Complaint.GetComplaintByIdAsync(complaintId, trackChanges);
            if (complaint == null)
            {
                _logger.LogWarn($"Complaint with Id = {complaintId} not found.");
                return null;
            }

            var complaintDto = _mapper.Map<ComplaintDto>(complaint);
            _logger.LogInfo($"Complaint with Id = {complaintId} fetched successfully.");
            return complaintDto;
        }


        public async Task<IEnumerable<ComplaintDto>> GetComplaintsByUserAsync(int userId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching complaints for user with Id = {userId}.");

            var complaints = await _repository.Complaint.GetComplaintsByUserAsync(userId, trackChanges);
            if (complaints == null || !complaints.Any())
            {
                _logger.LogWarn($"No complaints found for user with Id = {userId}.");
                return Enumerable.Empty<ComplaintDto>();
            }

            var complaintsDto = _mapper.Map<IEnumerable<ComplaintDto>>(complaints);
            _logger.LogInfo($"{complaintsDto.Count()} complaint(s) fetched for user with Id = {userId}.");
            return complaintsDto;
        }


        public async Task<IEnumerable<ComplaintDto>> GetComplaintsByProductAsync(int productId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching complaints for product with Id = {productId}.");

            var complaints = await _repository.Complaint.GetComplaintsByProductAsync(productId, trackChanges);
            if (complaints == null || !complaints.Any())
            {
                _logger.LogWarn($"No complaints found for product with Id = {productId}.");
                return Enumerable.Empty<ComplaintDto>();
            }

            var complaintsDto = _mapper.Map<IEnumerable<ComplaintDto>>(complaints);
            _logger.LogInfo($"{complaintsDto.Count()} complaint(s) fetched for product with Id = {productId}.");
            return complaintsDto;
        }

        public async Task CreateComplaintAsync(ComplaintCreateDto complaint)
        {
            if (complaint == null)
            {
                _logger.LogError("CreateComplaintAsync: ComplaintDto object is null.");
                return;
            }

            _logger.LogInfo($"Creating a new complaint by UserId = {complaint.UserId} for ProductId = {complaint.ProductId}.");

            // DTO -> Entity
            var complaintEntity = _mapper.Map<Complaint>(complaint);
            complaintEntity.CreatedAt = DateTime.Now;
            complaintEntity.UpdatedAt = DateTime.Now;
            complaintEntity.Status = "pending";

            // Repository üzerinden ekle
            _repository.Complaint.CreateComplaint(complaintEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Complaint created successfully with Id = {complaintEntity.Id}.");
        }

        public void  DeleteComplaint(int complaintId)
        {
            _logger.LogInfo($"Attempting to delete complaint with Id = {complaintId}.");

            var complaintEntity = _repository.Complaint.GetComplaintByIdAsync(complaintId, trackChanges: true).Result;
            if (complaintEntity == null)
            {
                _logger.LogWarn($"Complaint with Id = {complaintId} not found. Deletion canceled.");
                return;
            }

            _repository.Complaint.DeleteComplaint(complaintEntity);
            _repository.SaveAsync();

            _logger.LogInfo($"Complaint with Id = {complaintId} deleted successfully.");
        }
    }
}
