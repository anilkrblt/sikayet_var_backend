using Entities.Models;

namespace Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges);
        Task<Product> GetProductByIdAsync(int productId, bool trackChanges);
        Task<IEnumerable<Product>> GetProductsByBrandAsync(int brandId, bool trackChanges);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId, bool trackChanges);
        void CreateProduct(Product product);
        void DeleteProduct(Product product);
    }
}
