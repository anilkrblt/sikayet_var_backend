using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(bool trackChanges);
        Task<CategoryDto> GetCategoryByIdAsync(int categoryId, bool trackChanges);
        Task CreateCategoryAsync(CategoryDto category);
        Task DeleteCategoryAsync(int categoryId);
    }
}
