using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _serviceManager.CategoryService.GetAllCategoriesAsync(trackChanges: false);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id, trackChanges: false);
            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryCreateDto category)
        {
            if (category is null)
                return BadRequest("Category object is null");

            // Servisten dönen CategoryDto
            var createdCategory =  _serviceManager.CategoryService.CreateCategoryAsync(category);

            // Oluşturulan Category'i döndür
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _serviceManager.CategoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
