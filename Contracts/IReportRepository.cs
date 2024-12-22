using Entities.Models;

namespace Contracts
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllReportsAsync(bool trackChanges);
        Task<Report> GetReportByIdAsync(int reportId, bool trackChanges);
        Task<IEnumerable<Report>> GetReportsByReporterAsync(int reporterUserId, bool trackChanges);
        void CreateReport(Report report);
        void DeleteReport(Report report);
    }
}
