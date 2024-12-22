using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync(bool trackChanges);
        Task<BrandDto> GetBrandByIdAsync(int brandId, bool trackChanges);
        Task CreateBrandAsync(BrandDto brand);
        Task DeleteBrandAsync(int brandId);
    }
}
