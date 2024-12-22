using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public BrandsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _serviceManager.BrandService.GetAllBrandsAsync(trackChanges: false);
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var brand = await _serviceManager.BrandService.GetBrandByIdAsync(id, trackChanges: false);
            if (brand is null)
                return NotFound();

            return Ok(brand);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromBody] BrandDto brand)
        {
            if (brand is null)
                return BadRequest("Brand object is null");

            await _serviceManager.BrandService.CreateBrandAsync(brand);
            return CreatedAtAction(nameof(GetBrandById), new { id = brand.Id }, brand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            await _serviceManager.BrandService.DeleteBrandAsync(id);
            return NoContent();
        }
    }
}
