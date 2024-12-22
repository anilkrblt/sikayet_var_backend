using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(bool trackChanges);
        Task<ProductDto> GetProductByIdAsync(int productId, bool trackChanges);
        Task<IEnumerable<ProductDto>> GetProductsByBrandAsync(int brandId, bool trackChanges);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId, bool trackChanges);
        Task CreateProductAsync(ProductDto product);
        Task DeleteProductAsync(int productId);
    }
}
