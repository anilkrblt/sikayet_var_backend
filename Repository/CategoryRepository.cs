using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(c => c.Id)
                .ToListAsync();

        public async Task<Category> GetCategoryByIdAsync(int categoryId, bool trackChanges) =>
            await FindByCondition(c => c.Id == categoryId, trackChanges).SingleOrDefaultAsync();

        public void CreateCategory(Category category) => Create(category);
        public void DeleteCategory(Category category) => Delete(category);
    }
}
