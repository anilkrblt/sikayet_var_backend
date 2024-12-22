using Entities.Models;

namespace Contracts
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllBrandsAsync(bool trackChanges);
        Task<Brand> GetBrandByIdAsync(int brandId, bool trackChanges);
        void CreateBrand(Brand brand);
        void DeleteBrand(Brand brand);
    }
}
