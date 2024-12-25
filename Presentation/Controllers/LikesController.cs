using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/like")]
    public class LikesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public LikesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLikes()
        {
            var likes = await _serviceManager.LikeService.GetAllLikesAsync(trackChanges: false);
            return Ok(likes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLikeById(int id)
        {
            var like = await _serviceManager.LikeService.GetLikeByIdAsync(id, trackChanges: false);
            if (like is null)
                return NotFound();

            return Ok(like);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetLikesByUser(int userId)
        {
            var likes = await _serviceManager.LikeService.GetLikesByUserAsync(userId, trackChanges: false);
            return Ok(likes);
        }

        [HttpGet("complaint/{complaintId}")]
        public async Task<IActionResult> GetLikesByComplaint(int complaintId)
        {
            var likes = await _serviceManager.LikeService.GetLikesByComplaintAsync(complaintId, trackChanges: false);
            return Ok(likes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLike([FromBody] LikeCreateDto like)
        {
            if (like is null)
                return BadRequest("Like object is null");

            var likeCreated = _serviceManager.LikeService.CreateLikeAsync(like);
            return CreatedAtAction(nameof(GetLikeById), new { id = likeCreated.Id }, likeCreated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            await _serviceManager.LikeService.DeleteLikeAsync(id);
            return NoContent();
        }
    }
}
