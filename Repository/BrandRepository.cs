using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class BrandRepository : RepositoryBase<Brand>, IBrandRepository
    {
        public BrandRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(b => b.Id)
                .ToListAsync();

        public async Task<Brand> GetBrandByIdAsync(int brandId, bool trackChanges) =>
            await FindByCondition(b => b.Id == brandId, trackChanges).SingleOrDefaultAsync();

        public void CreateBrand(Brand brand) => Create(brand);
        public void DeleteBrand(Brand brand) => Delete(brand);
    }
}
