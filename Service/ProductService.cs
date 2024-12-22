using AutoMapper;
using Contracts;                       // IRepositoryManager, ILoggerManager vb.
using Entities.Models;                 // Product entity
using Service.Contracts;               // IProductService
using Shared.DataTransferObjects;      // ProductDto

namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ProductService(IRepositoryManager repository,
                              ILoggerManager logger,
                              IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm ürünlerin DTO listesini döner.
        /// </summary>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>ProductDto listesi</returns>
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(bool trackChanges)
        {
            _logger.LogInfo("Fetching all products from the database.");

            var products = await _repository.Product.GetAllProductsAsync(trackChanges);
            if (products == null || !products.Any())
            {
                _logger.LogWarn("No products found in the database.");
                return Enumerable.Empty<ProductDto>();
            }

            // Entity -> DTO
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            _logger.LogInfo($"{productsDto.Count()} product(s) fetched successfully.");

            return productsDto;
        }

        /// <summary>
        /// Belirtilen Id'ye sahip ürünü (Product) DTO olarak döner.
        /// </summary>
        /// <param name="productId">Aranacak ürünün ID'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>ProductDto veya null</returns>
        public async Task<ProductDto> GetProductByIdAsync(int productId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching product with Id = {productId}.");

            var product = await _repository.Product.GetProductByIdAsync(productId, trackChanges);
            if (product == null)
            {
                _logger.LogWarn($"Product with Id = {productId} not found.");
                return null;
            }

            var productDto = _mapper.Map<ProductDto>(product);
            _logger.LogInfo($"Product with Id = {productId} fetched successfully.");
            return productDto;
        }

        /// <summary>
        /// Belirtilen markaya (brandId) ait ürünleri DTO listesi olarak döner.
        /// </summary>
        /// <param name="brandId">Ürünleri getirilecek markanın Id'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>ProductDto listesi</returns>
        public async Task<IEnumerable<ProductDto>> GetProductsByBrandAsync(int brandId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching products for brand with Id = {brandId}.");

            var products = await _repository.Product.GetProductsByBrandAsync(brandId, trackChanges);
            if (products == null || !products.Any())
            {
                _logger.LogWarn($"No products found for brand with Id = {brandId}.");
                return Enumerable.Empty<ProductDto>();
            }

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            _logger.LogInfo($"{productsDto.Count()} product(s) fetched for brand with Id = {brandId}.");

            return productsDto;
        }

        /// <summary>
        /// Belirtilen kategoriye (categoryId) ait ürünleri DTO listesi olarak döner.
        /// </summary>
        /// <param name="categoryId">Ürünleri getirilecek kategorinin Id'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>ProductDto listesi</returns>
        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching products for category with Id = {categoryId}.");

            var products = await _repository.Product.GetProductsByCategoryAsync(categoryId, trackChanges);
            if (products == null || !products.Any())
            {
                _logger.LogWarn($"No products found for category with Id = {categoryId}.");
                return Enumerable.Empty<ProductDto>();
            }

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            _logger.LogInfo($"{productsDto.Count()} product(s) fetched for category with Id = {categoryId}.");

            return productsDto;
        }

        /// <summary>
        /// Yeni bir ürün (Product) oluşturur.
        /// </summary>
        /// <param name="product">Oluşturulacak ürünün DTO nesnesi</param>
        public async Task CreateProductAsync(ProductDto product)
        {
            if (product == null)
            {
                _logger.LogError("CreateProductAsync: ProductDto object is null.");
                return;
            }

            _logger.LogInfo($"Creating a new product with Name = '{product.Name}'.");

            // DTO -> Entity
            var productEntity = _mapper.Map<Product>(product);

            // Repository üzerinden ekle
            _repository.Product.CreateProduct(productEntity);

            // Değişiklikleri kalıcı hâle getir
            await _repository.SaveAsync();

            _logger.LogInfo($"Product created successfully with Id = {productEntity.Id}.");
        }

        /// <summary>
        /// Belirtilen Id'ye sahip ürünü siler.
        /// </summary>
        /// <param name="productId">Silinecek ürünün Id'si</param>
        public async Task DeleteProductAsync(int productId)
        {
            _logger.LogInfo($"Attempting to delete product with Id = {productId}.");

            // Silinecek ürünü getir
            var productEntity = await _repository.Product.GetProductByIdAsync(productId, trackChanges: false);
            if (productEntity == null)
            {
                _logger.LogWarn($"Product with Id = {productId} not found. Deletion canceled.");
                return;
            }

            // Repository üzerinden sil
            _repository.Product.DeleteProduct(productEntity);
            await _repository.SaveAsync();

            _logger.LogInfo($"Product with Id = {productId} deleted successfully.");
        }
    }
}
