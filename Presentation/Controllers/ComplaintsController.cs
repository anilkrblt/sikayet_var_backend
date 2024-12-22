using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ComplaintsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComplaints()
        {
            var complaints = await _serviceManager.ComplaintService.GetAllComplaintsAsync(trackChanges: false);
            return Ok(complaints);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComplaintById(int id)
        {
            var complaint = await _serviceManager.ComplaintService.GetComplaintByIdAsync(id, trackChanges: false);
            if (complaint is null)
                return NotFound();

            return Ok(complaint);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetComplaintsByUser(int userId)
        {
            var complaints = await _serviceManager.ComplaintService.GetComplaintsByUserAsync(userId, trackChanges: false);
            return Ok(complaints);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetComplaintsByProduct(int productId)
        {
            var complaints = await _serviceManager.ComplaintService.GetComplaintsByProductAsync(productId, trackChanges: false);
            return Ok(complaints);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComplaint([FromBody] ComplaintDto complaint)
        {
            if (complaint is null)
                return BadRequest("Complaint object is null");

            await _serviceManager.ComplaintService.CreateComplaintAsync(complaint);
            return CreatedAtAction(nameof(GetComplaintById), new { id = complaint.Id }, complaint);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComplaint(int id)
        {
            await _serviceManager.ComplaintService.DeleteComplaintAsync(id);
            return NoContent();
        }
    }
}
