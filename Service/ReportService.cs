using AutoMapper;
using Contracts;                 // IRepositoryManager ve ILoggerManager arayüzleri
using Entities.Models;           // Report entity
using Service.Contracts;         // IReportService interface
using Shared.DataTransferObjects; // ReportDto

namespace Service
{
    public class ReportService : IReportService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ReportService(IRepositoryManager repository,
                             ILoggerManager logger,
                             IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm raporları DTO şeklinde döner.
        /// </summary>
        /// <param name="trackChanges">EF Core'un değişiklikleri takip edip etmeyeceği</param>
        /// <returns>Raporların DTO listesi</returns>
        public async Task<IEnumerable<ReportDto>> GetAllReportsAsync(bool trackChanges)
        {
            _logger.LogInfo("Fetching all reports from the database via repository.");

            // Repository üzerinden raporları çek
            var reports = await _repository.Report.GetAllReportsAsync(trackChanges);

            if (reports == null || !reports.Any())
            {
                _logger.LogWarn("No reports found in the database.");
                return Enumerable.Empty<ReportDto>();
            }

            // Entity -> DTO
            var reportsDto = _mapper.Map<IEnumerable<ReportDto>>(reports);

            _logger.LogInfo($"{reportsDto.Count()} report(s) fetched successfully.");
            return reportsDto;
        }

        /// <summary>
        /// ID bazında tekil bir raporu DTO olarak döner.
        /// </summary>
        /// <param name="reportId">Aranacak raporun ID'si</param>
        /// <param name="trackChanges">EF Core'un değişiklikleri takip edip etmeyeceği</param>
        /// <returns>Tekil bir rapor DTO veya null</returns>
        public async Task<ReportDto> GetReportByIdAsync(int reportId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching report with Id = {reportId}");

            var report = await _repository.Report.GetReportByIdAsync(reportId, trackChanges);
            if (report == null)
            {
                _logger.LogWarn($"Report with Id = {reportId} not found.");
                return null;
            }

            var reportDto = _mapper.Map<ReportDto>(report);
            _logger.LogInfo($"Report with Id = {reportId} fetched successfully.");

            return reportDto;
        }

        /// <summary>
        /// Belirli bir kullanıcı (reporterUserId) tarafından oluşturulan raporları döner
        /// </summary>
        /// <param name="reporterUserId">Raporları getirilmek istenen kullanıcının Id'si</param>
        /// <param name="trackChanges">EF Core'un değişiklikleri takip edip etmeyeceği</param>
        /// <returns>Belirtilen kullanıcıya ait raporların DTO listesi</returns>
        public async Task<IEnumerable<ReportDto>> GetReportsByReporterAsync(int reporterUserId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching reports created by user with Id = {reporterUserId}");

            var reports = await _repository.Report.GetReportsByReporterAsync(reporterUserId, trackChanges);
            if (reports == null || !reports.Any())
            {
                _logger.LogWarn($"No reports found for user with Id = {reporterUserId}.");
                return Enumerable.Empty<ReportDto>();
            }

            var reportsDto = _mapper.Map<IEnumerable<ReportDto>>(reports);

            _logger.LogInfo($"{reportsDto.Count()} report(s) fetched for user with Id = {reporterUserId}.");
            return reportsDto;
        }

        /// <summary>
        /// Yeni rapor oluşturur.
        /// </summary>
        /// <param name="reportDto">Oluşturulacak raporun DTO'su</param>
        public async Task CreateReportAsync(ReportDto reportDto)
        {
            if (reportDto == null)
            {
                _logger.LogError("CreateReportAsync: ReportDto is null. Cannot create a report.");
                return;
            }

            _logger.LogInfo($"Creating a new report with Title = '{reportDto.Id}'");

            // DTO -> Entity
            var reportEntity = _mapper.Map<Report>(reportDto);

            // Repository aracılığıyla ekleme
            _repository.Report.CreateReport(reportEntity);

            // Değişiklikleri veritabanına kaydet
            await _repository.SaveAsync();

            _logger.LogInfo($"Report created successfully with Id = {reportEntity.Id}.");
        }

        /// <summary>
        /// Belirtilen Id'ye sahip raporu siler.
        /// </summary>
        /// <param name="reportId">Silinecek raporun Id'si</param>
        public async Task DeleteReportAsync(int reportId)
        {
            _logger.LogInfo($"Attempting to delete report with Id = {reportId}.");

            // Silinecek raporu getir
            var reportEntity = await _repository.Report.GetReportByIdAsync(reportId, trackChanges: false);
            if (reportEntity == null)
            {
                _logger.LogWarn($"Report with Id = {reportId} not found. Deletion canceled.");
                return;
            }

            // Repository üzerinden sil
            _repository.Report.DeleteReport(reportEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Report with Id = {reportId} was deleted successfully.");
        }
    }
}
