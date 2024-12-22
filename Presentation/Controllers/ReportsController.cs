using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ReportsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _serviceManager.ReportService.GetAllReportsAsync(trackChanges: false);
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(int id)
        {
            var report = await _serviceManager.ReportService.GetReportByIdAsync(id, trackChanges: false);
            if (report is null)
                return NotFound();

            return Ok(report);
        }

        [HttpGet("reporter/{reporterUserId}")]
        public async Task<IActionResult> GetReportsByReporter(int reporterUserId)
        {
            var reports = await _serviceManager.ReportService.GetReportsByReporterAsync(reporterUserId, trackChanges: false);
            return Ok(reports);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportDto report)
        {
            if (report is null)
                return BadRequest("Report object is null");

            await _serviceManager.ReportService.CreateReportAsync(report);
            return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            await _serviceManager.ReportService.DeleteReportAsync(id);
            return NoContent();
        }
    }
}
