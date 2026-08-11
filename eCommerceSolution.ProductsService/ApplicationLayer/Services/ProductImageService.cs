using ApplicationLayer.DTOs.ProductImages;
using ApplicationLayer.ServiceContracts;
using AutoMapper;
using BusinessLogicLayer.Entities;
using BusinessLogicLayer.RepositoryContracts;

namespace ApplicationLayer.Services;

public class ProductImageService : IProductImageService
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductImageService(
        IProductImageRepository productImageRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productImageRepository = productImageRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductImageResponseDto> AddImageAsync(CreateProductImageRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
            throw new KeyNotFoundException("Product not found.");

        var image = _mapper.Map<ProductImage>(request);

        await _productImageRepository.AddAsync(image);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductImageResponseDto>(image);
    }

    public async Task<bool> DeleteImageAsync(Guid imageId)
    {
        var images = await _productImageRepository.GetImagesByProductIdAsync(Guid.Empty);

        var image = images.FirstOrDefault(i => i.Id == imageId);

        if (image is null)
            throw new KeyNotFoundException("Image not found.");

        _productImageRepository.Delete(image);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ProductImageResponseDto>> GetImagesByProductIdAsync(Guid productId)
    {
        var images = await _productImageRepository.GetImagesByProductIdAsync(productId);

        return _mapper.Map<IEnumerable<ProductImageResponseDto>>(images);
    }
}
