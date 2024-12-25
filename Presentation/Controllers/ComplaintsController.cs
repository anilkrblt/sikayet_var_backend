using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/complaint")]
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
        public IActionResult CreateComplaint([FromBody] ComplaintCreateDto complaint)
        {
            if (complaint is null)
                return BadRequest("Complaint object is null");

            var complaintCreated =  _serviceManager.ComplaintService.CreateComplaintAsync(complaint);
            return CreatedAtAction(nameof(GetComplaintById), new { id = complaintCreated.Id }, complaintCreated);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteComplaint(int id)
        {
            _serviceManager.ComplaintService.DeleteComplaint(id);
            return NoContent();
        }
        /*
                [HttpPatch("{id}/status")]
                public async Task<IActionResult> UpdateComplaintStatus(int id, [FromBody] ComplaintStatusUpdateDto statusUpdate)
                {
                    if (statusUpdate is null)
                        return BadRequest("Status update object is null");

                    await _serviceManager.ComplaintService.UpdateComplaintStatusAsync(id, statusUpdate);
                    return NoContent();
                }
                [Authorize(Roles = "Admin")]
                [HttpDelete("{id}")]
                public async Task<IActionResult> DeleteComplaintByAdmin(int id)
                {
                    await _serviceManager.ComplaintService.DeleteComplaintByAdminAsync(id);
                    return NoContent();
                }

                [HttpGet("search")]
                public async Task<IActionResult> SearchComplaints([FromQuery] string keyword)
                {
                    if (string.IsNullOrWhiteSpace(keyword))
                        return BadRequest("Keyword cannot be empty");

                    var complaints = await _serviceManager.ComplaintService.SearchComplaintsAsync(keyword, trackChanges: false);
                    return Ok(complaints);
                }*/


    }
}
/*







*/