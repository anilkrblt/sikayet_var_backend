using Entities.Models;

namespace Contracts
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
        Task<Category> GetCategoryByIdAsync(int categoryId, bool trackChanges);
        void CreateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
