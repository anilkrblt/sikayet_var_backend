using AutoMapper;
using Contracts;                   // ILoggerManager, IRepositoryManager vb.
using Entities.Models;             // Category entity
using Service.Contracts;           // ICategoryService
using Shared.DataTransferObjects;   // CategoryDto

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

        /// <summary>
        /// Tüm kategorilerin (Category) DTO listesini döner
        /// </summary>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>Kategori DTO listesi</returns>
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(bool trackChanges)
        {
            _logger.LogInfo("Fetching all categories from the database.");

            // 1) Repository'den kategorileri çek
            var categories = await _repository.Category.GetAllCategoriesAsync(trackChanges);

            if (categories == null || !categories.Any())
            {
                _logger.LogWarn("No categories found in the database.");
                return Enumerable.Empty<CategoryDto>();
            }

            // 2) Category -> CategoryDto dönüştür
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            _logger.LogInfo($"{categoriesDto.Count()} category(ies) fetched successfully.");
            return categoriesDto;
        }

        /// <summary>
        /// ID bazında tekil bir kategorinin DTO'sunu döner
        /// </summary>
        /// <param name="categoryId">Aranacak kategorinin ID'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>CategoryDto veya null</returns>
        public async Task<CategoryDto> GetCategoryByIdAsync(int categoryId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching category with Id = {categoryId}.");

            // 1) Repository'den ilgili category'yi çek
            var category = await _repository.Category.GetCategoryByIdAsync(categoryId, trackChanges);
            if (category == null)
            {
                _logger.LogWarn($"Category with Id = {categoryId} not found.");
                return null;
            }

            // 2) Category -> CategoryDto dönüştür
            var categoryDto = _mapper.Map<CategoryDto>(category);

            _logger.LogInfo($"Category with Id = {categoryId} fetched successfully.");
            return categoryDto;
        }

        /// <summary>
        /// Yeni bir kategori (Category) oluşturur
        /// </summary>
        /// <param name="category">Oluşturulacak kategorinin DTO nesnesi</param>
        public async Task CreateCategoryAsync(CategoryDto category)
        {
            if (category == null)
            {
                _logger.LogError("CreateCategoryAsync: CategoryDto object is null.");
                return;
            }

            _logger.LogInfo($"Creating a new category with Name = '{category.Name}'.");

            // 1) CategoryDto -> Category entity
            var categoryEntity = _mapper.Map<Category>(category);

            // 2) Repository aracılığıyla ekleme
            _repository.Category.CreateCategory(categoryEntity);

            // 3) Kaydet
            await _repository.SaveAsync();

            _logger.LogInfo($"Category created successfully with Id = {categoryEntity.Id}.");
        }

        /// <summary>
        /// Belirtilen Id'ye sahip kategoriyi siler
        /// </summary>
        /// <param name="categoryId">Silinecek kategorinin Id'si</param>
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
