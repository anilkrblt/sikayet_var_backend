using AutoMapper;
using Contracts;                 
using Entities.Exceptions;
using Entities.Models;         
using Service.Contracts;           
using Shared.DataTransferObjects;   

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

        
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync(bool trackChanges)
        {
            _logger.LogInfo("Fetching all brands from the database.");

            var brands = await _repository.Brand.GetAllBrandsAsync(trackChanges);

            if (brands == null || !brands.Any())
            {
                _logger.LogWarn("No brands found in the database.");
                return Enumerable.Empty<BrandDto>();
            }

            var brandsDto = _mapper.Map<IEnumerable<BrandDto>>(brands);
            _logger.LogInfo($"{brandsDto.Count()} brand(s) fetched successfully from the database.");
            return brandsDto;
        }

        public async Task<BrandDto> GetBrandByIdAsync(int brandId, bool trackChanges)
        {
            _logger.LogInfo($"Fetching brand with Id = {brandId}.");

            var brand = await _repository.Brand.GetBrandByIdAsync(brandId, trackChanges);
            if (brand == null)
            {
                _logger.LogWarn($"Brand with Id = {brandId} not found.");
                return null;
            }

            var brandDto = _mapper.Map<BrandDto>(brand);

            _logger.LogInfo($"Brand with Id = {brandId} fetched successfully.");
            return brandDto;
        }
        public async Task CreateBrandAsync(BrandCreateDto brandDto)
        {
            if (brandDto == null)
            {
                _logger.LogError("CreateBrandAsync: BrandDto object is null.");
                throw new BrandNotFoundException();
            }

            _logger.LogInfo($"Creating a new brand with Name = '{brandDto.Name}'.");

            var brandEntity = _mapper.Map<Brand>(brandDto);
            brandEntity.CreatedAt = DateTime.Now;
            brandEntity.UpdatedAt = DateTime.Now;
            _repository.Brand.CreateBrand(brandEntity);
            await _repository.SaveAsync();
            _logger.LogInfo($"Brand created successfully with Id = {brandEntity.Id}.");
        }

        public async Task DeleteBrandAsync(int brandId)
        {
            _logger.LogInfo($"Attempting to delete brand with Id = {brandId}.");

            var brandEntity = await _repository.Brand.GetBrandByIdAsync(brandId, trackChanges: false);
            if (brandEntity == null)
            {
                _logger.LogWarn($"Brand with Id = {brandId} not found. Deletion canceled.");
                return;
            }

            _repository.Brand.DeleteBrand(brandEntity);
            await _repository.SaveAsync();
            _logger.LogInfo($"Brand with Id = {brandId} deleted successfully.");
        }
    }
}
