using AutoMapper;
using Contracts;                   // ILoggerManager, IRepositoryManager arayüzlerinin burada olduğunu varsayıyoruz
using Entities.Models;             // Brand entity
using Service.Contracts;           // IBrandService arayüzü
using Shared.DataTransferObjects;   // BrandDto

namespace Service
{
    public class BrandService : IBrandService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public BrandService(IRepositoryManager repository,
                            ILoggerManager logger,
                            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm markaların (Brand) DTO listesini döner
        /// </summary>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>BrandDto listesi</returns>
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync(bool trackChanges)
        {
            _logger.LogInfo("Fetching all brands from the database.");

            // 1) Repository'den markaları çek
            var brands = await _repository.Brand.GetAllBrandsAsync(trackChanges);

            if (brands == null || !brands.Any())
            {
                _logger.LogWarn("No brands found in the database.");
                return Enumerable.Empty<BrandDto>();
            }

            // 2) Brand -> BrandDto dönüştür
            var brandsDto = _mapper.Map<IEnumerable<BrandDto>>(brands);

            _logger.LogInfo($"{brandsDto.Count()} brand(s) fetched successfully from the database.");

            return brandsDto;
        }

        /// <summary>
        /// ID bazında tekil bir markanın DTO'sunu döner
        /// </summary>
        /// <param name="brandId">Aranacak markanın ID'si</param>
        /// <param name="trackChanges">EF Core değişiklik izleme (tracking) seçeneği</param>
        /// <returns>BrandDto veya null</returns>
        public async Task<BrandDto> GetBrandByIdAsync(int brandId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching brand with Id = {brandId}.");

            // 1) Repository'den ilgili brand'i çek
            var brand = await _repository.Brand.GetBrandByIdAsync(brandId, trackChanges);
            if (brand == null)
            {
                _logger.LogWarn($"Brand with Id = {brandId} not found.");
                return null;
            }

            // 2) Brand -> BrandDto dönüştür
            var brandDto = _mapper.Map<BrandDto>(brand);

            _logger.LogInfo($"Brand with Id = {brandId} fetched successfully.");
            return brandDto;
        }

        /// <summary>
        /// Yeni bir marka (Brand) oluşturur
        /// </summary>
        /// <param name="brandDto">Oluşturulacak markanın DTO nesnesi</param>
        public async Task CreateBrandAsync(BrandDto brandDto)
        {
            if (brandDto == null)
            {
                _logger.LogError("CreateBrandAsync: BrandDto object is null.");
                return;
            }

            _logger.LogInfo($"Creating a new brand with Name = '{brandDto.Name}'.");

            // 1) BrandDto -> Brand entity
            var brandEntity = _mapper.Map<Brand>(brandDto);

            // 2) Repository aracılığıyla ekleme
            _repository.Brand.CreateBrand(brandEntity);

            // 3) Kaydet
            await _repository.SaveAsync();

            _logger.LogInfo($"Brand created successfully with Id = {brandEntity.Id}.");
        }

        /// <summary>
        /// Belirtilen Id'ye sahip markayı siler
        /// </summary>
        /// <param name="brandId">Silinecek markanın Id'si</param>
        public async Task DeleteBrandAsync(int brandId)
        {
            _logger.LogInfo($"Attempting to delete brand with Id = {brandId}.");

            // 1) Önce brand var mı diye kontrol edelim
            var brandEntity = await _repository.Brand.GetBrandByIdAsync(brandId, trackChanges: false);
            if (brandEntity == null)
            {
                _logger.LogWarn($"Brand with Id = {brandId} not found. Deletion canceled.");
                return;
            }

            // 2) Repository üzerinden sil
            _repository.Brand.DeleteBrand(brandEntity);

            // 3) Kaydet
            await _repository.SaveAsync();

            _logger.LogInfo($"Brand with Id = {brandId} deleted successfully.");
        }
    }
}
