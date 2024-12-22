using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(p => p.Name)
                .ToListAsync();

        public async Task<Product> GetProductByIdAsync(int productId, bool trackChanges) =>
            await FindByCondition(p => p.Id == productId, trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(int brandId, bool trackChanges) =>
            await FindByCondition(p => p.BrandId == brandId, trackChanges).ToListAsync();

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId, bool trackChanges) =>
            await FindByCondition(p => p.CategoryId == categoryId, trackChanges).ToListAsync();

        public void CreateProduct(Product product) => Create(product);
        public void DeleteProduct(Product product) => Delete(product);
    }
}
