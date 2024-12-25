using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CommentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("complaint/{complaintId}")]
        public async Task<IActionResult> GetCommentsByComplaint(int complaintId)
        {
            var comments = await _serviceManager.CommentService.GetCommentsByComplaintAsync(complaintId, trackChanges: false);
            return Ok(comments);
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCommentsByUser(int userId)
        {
            var comments = await _serviceManager.CommentService.GetCommentsByUserAsync(userId, trackChanges: false);
            return Ok(comments);
        }


        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto comment)
        {

            var createdComment = await _serviceManager.CommentService.CreateCommentAsync(comment);
            return RedirectToAction("GetCommentById", new { id = createdComment.Id });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value);
            await _serviceManager.CommentService.DeleteCommentAsync(id, userId);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteCommentByAdmin(int id)
        {
            await _serviceManager.CommentService.DeleteCommentByAdminAsync(id);
            return NoContent();
        }
    }
}
