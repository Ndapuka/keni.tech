using ApplicationLayer.DTOs.ProductImages;

namespace ApplicationLayer.ServiceContracts;

public interface IProductImageService
{
    Task<ProductImageResponseDto> AddImageAsync(CreateProductImageRequestDto request);

    Task<bool> DeleteImageAsync(Guid imageId);

    Task<IEnumerable<ProductImageResponseDto>> GetImagesByProductIdAsync(Guid productId);
}