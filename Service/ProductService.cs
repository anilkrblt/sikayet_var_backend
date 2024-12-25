using AutoMapper;
using Contracts;                      
using Entities.Models;               
using Service.Contracts;              
using Shared.DataTransferObjects;      

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

       
        public async Task CreateProductAsync(ProductCreateDto product)
        {
            if (product == null)
            {
                _logger.LogError("CreateProductAsync: ProductDto object is null.");
                return;
            }

            _logger.LogInfo($"Creating a new product with Name = '{product.Name}'.");

            // DTO -> Entity
            var productEntity = _mapper.Map<Product>(product);
            productEntity.CreatedAt = DateTime.Now;
            productEntity.UpdatedAt = DateTime.Now;

            // Repository üzerinden ekle
            _repository.Product.CreateProduct(productEntity);

            // Değişiklikleri kalıcı hâle getir
            await _repository.SaveAsync();

            _logger.LogInfo($"Product created successfully with Id = {productEntity.Id}.");
        }

    
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
