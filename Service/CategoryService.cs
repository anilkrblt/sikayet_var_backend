using AutoMapper;
using Contracts;                  
using Entities.Models;             
using Service.Contracts;           
using Shared.DataTransferObjects;   

namespace Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CategoryService(IRepositoryManager repository,
                               ILoggerManager logger,
                               IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(bool trackChanges)
        {
            _logger.LogInfo("Fetching all categories from the database.");

            var categories = await _repository.Category.GetAllCategoriesAsync(trackChanges);

            if (categories == null || !categories.Any())
            {
                _logger.LogWarn("No categories found in the database.");
                return Enumerable.Empty<CategoryDto>();
            }

            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            _logger.LogInfo($"{categoriesDto.Count()} category(ies) fetched successfully.");
            return categoriesDto;
        }

        
        public async Task<CategoryDto> GetCategoryByIdAsync(int categoryId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching category with Id = {categoryId}.");

            var category = await _repository.Category.GetCategoryByIdAsync(categoryId, trackChanges);
            if (category == null)
            {
                _logger.LogWarn($"Category with Id = {categoryId} not found.");
                return null;
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);

            _logger.LogInfo($"Category with Id = {categoryId} fetched successfully.");
            return categoryDto;
        }

        public async Task CreateCategoryAsync(CategoryCreateDto category)
        {
            if (category == null)
            {
                _logger.LogError("CreateCategoryAsync: CategoryDto object is null.");
                return;
            }

            _logger.LogInfo($"Creating a new category with Name = '{category.Name}'.");

            // 1) CategoryDto -> Category entity
            var categoryEntity = _mapper.Map<Category>(category);
            categoryEntity.CreatedAt = DateTime.Now;
            categoryEntity.UpdatedAt = DateTime.Now;

            // 2) Repository aracılığıyla ekleme
            _repository.Category.CreateCategory(categoryEntity);

            // 3) Kaydet
            await _repository.SaveAsync();

            _logger.LogInfo($"Category created successfully with Id = {categoryEntity.Id}.");
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            _logger.LogInfo($"Attempting to delete category with Id = {categoryId}.");

            // 1) Önce category var mı diye kontrol edelim
            var categoryEntity = await _repository.Category.GetCategoryByIdAsync(categoryId, trackChanges: false);
            if (categoryEntity == null)
            {
                _logger.LogWarn($"Category with Id = {categoryId} not found. Deletion canceled.");
                return;
            }

            // 2) Repository üzerinden sil
            _repository.Category.DeleteCategory(categoryEntity);

            // 3) Kaydet
            await _repository.SaveAsync();

            _logger.LogInfo($"Category with Id = {categoryId} deleted successfully.");
        }
    }
}
