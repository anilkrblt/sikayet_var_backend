using AutoMapper;
using Contracts;                   // ILoggerManager, IRepositoryManager vb.
using Entities.Models;             // Complaint entity
using Service.Contracts;           // IComplaintService
using Shared.DataTransferObjects;   // ComplaintDto

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

        /// <summary>
        /// Tüm şikayetlerin (Complaint) DTO listesini döner.
        /// </summary>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>ComplaintDto listesi</returns>
        public async Task<IEnumerable<ComplaintDto>> GetAllComplaintsAsync(bool trackChanges)
        {
            _logger.LogInfo("Fetching all complaints from the database.");

            var complaints = await _repository.Complaint.GetAllComplaintsAsync(trackChanges);
            if (complaints == null || !complaints.Any())
            {
                _logger.LogWarn("No complaints found in the database.");
                return Enumerable.Empty<ComplaintDto>();
            }

            // Entity -> DTO
            var complaintsDto = _mapper.Map<IEnumerable<ComplaintDto>>(complaints);
            _logger.LogInfo($"{complaintsDto.Count()} complaint(s) fetched successfully.");
            return complaintsDto;
        }

        /// <summary>
        /// Belirtilen Id'ye sahip şikayeti (Complaint) DTO olarak döner.
        /// </summary>
        /// <param name="complaintId">Aranacak şikayetin ID'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>ComplaintDto veya null</returns>
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

        /// <summary>
        /// Bir kullanıcıya (userId) ait tüm şikayetleri DTO listesi olarak döner.
        /// </summary>
        /// <param name="userId">Şikayetleri getirilecek kullanıcının Id'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>ComplaintDto listesi</returns>
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

        /// <summary>
        /// Bir ürüne (productId) ait tüm şikayetleri DTO listesi olarak döner.
        /// </summary>
        /// <param name="productId">Şikayetleri getirilecek ürünün Id'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>ComplaintDto listesi</returns>
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

        /// <summary>
        /// Yeni bir şikayet (Complaint) oluşturur.
        /// </summary>
        /// <param name="complaint">Oluşturulacak şikayetin DTO nesnesi</param>
        public async Task CreateComplaintAsync(ComplaintDto complaint)
        {
            if (complaint == null)
            {
                _logger.LogError("CreateComplaintAsync: ComplaintDto object is null.");
                return;
            }

            _logger.LogInfo($"Creating a new complaint by UserId = {complaint.UserId} for ProductId = {complaint.ProductId}.");

            // DTO -> Entity
            var complaintEntity = _mapper.Map<Complaint>(complaint);

            // Repository üzerinden ekle
            _repository.Complaint.CreateComplaint(complaintEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Complaint created successfully with Id = {complaintEntity.Id}.");
        }

        /// <summary>
        /// Belirtilen Id'ye sahip şikayeti siler.
        /// </summary>
        /// <param name="complaintId">Silinecek şikayetin Id'si</param>
        public async Task DeleteComplaintAsync(int complaintId)
        {
            _logger.LogInfo($"Attempting to delete complaint with Id = {complaintId}.");

            var complaintEntity = await _repository.Complaint.GetComplaintByIdAsync(complaintId, trackChanges: false);
            if (complaintEntity == null)
            {
                _logger.LogWarn($"Complaint with Id = {complaintId} not found. Deletion canceled.");
                return;
            }

            _repository.Complaint.DeleteComplaint(complaintEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Complaint with Id = {complaintId} deleted successfully.");
        }
    }
}
