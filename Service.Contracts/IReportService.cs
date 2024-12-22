using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IReportService
    {
        Task<IEnumerable<ReportDto>> GetAllReportsAsync(bool trackChanges);
        Task<ReportDto> GetReportByIdAsync(int reportId, bool trackChanges);
        Task<IEnumerable<ReportDto>> GetReportsByReporterAsync(int reporterUserId, bool trackChanges);
        Task CreateReportAsync(ReportDto report);
        Task DeleteReportAsync(int reportId);
    }
}
