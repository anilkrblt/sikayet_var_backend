using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/brands")]
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
        public IActionResult CreateBrand([FromBody] BrandCreateDto brand)
        {
            if (brand is null)
                return BadRequest("Brand object is null");

            var createdBrand = _serviceManager.BrandService.CreateBrandAsync(brand);

            return CreatedAtAction(nameof(GetBrandById), new { id = createdBrand.Id }, createdBrand);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            await _serviceManager.BrandService.DeleteBrandAsync(id);
            return NoContent();
        }
    }
}
