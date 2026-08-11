
using ApplicationLayer.DTOs.Products;

namespace ApplicationLayer.ServiceContracts;

public interface IProductService
{
    Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto request);

    Task<ProductResponseDto> UpdateProductAsync(UpdateProductRequestDto request);

    Task<bool> DeleteProductAsync(Guid productId);

    Task<ProductResponseDto?> GetProductByIdAsync(Guid productId);

    Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();

    Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(Guid categoryId);

    Task<IEnumerable<ProductResponseDto>> GetFeaturedProductsAsync();

    Task<IEnumerable<ProductResponseDto>> SearchProductsAsync(string keyword);
}