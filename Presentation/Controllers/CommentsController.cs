using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CommentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _serviceManager.CommentService.GetAllCommentsAsync(trackChanges: false);
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _serviceManager.CommentService.GetCommentByIdAsync(id, trackChanges: false);
            if (comment is null)
                return NotFound();

            return Ok(comment);
        }

        [HttpGet("complaint/{complaintId}")]
        public async Task<IActionResult> GetCommentsByComplaint(int complaintId)
        {
            var comments = await _serviceManager.CommentService.GetCommentsByComplaintAsync(complaintId, trackChanges: false);
            return Ok(comments);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCommentsByUser(int userId)
        {
            var comments = await _serviceManager.CommentService.GetCommentsByUserAsync(userId, trackChanges: false);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentDto comment)
        {
            if (comment is null)
                return BadRequest("Comment object is null");

            await _serviceManager.CommentService.CreateCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _serviceManager.CommentService.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}
