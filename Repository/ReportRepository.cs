using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ReportRepository : RepositoryBase<Report>, IReportRepository
    {
        public ReportRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();

        public async Task<Report> GetReportByIdAsync(int reportId, bool trackChanges) =>
            await FindByCondition(r => r.Id == reportId, trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Report>> GetReportsByReporterAsync(int reporterUserId, bool trackChanges) =>
            await FindByCondition(r => r.ReporterUserId == reporterUserId, trackChanges).ToListAsync();

        public void CreateReport(Report report) => Create(report);
        public void DeleteReport(Report report) => Delete(report);
    }
}
