using ApplicationLayer.DTOs.Categories;

namespace ApplicationLayer.ServiceContracts;

public interface ICategoryService
{
    Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryRequestDto request);

    Task<CategoryResponseDto> UpdateCategoryAsync(UpdateCategoryRequestDto request);

    Task<bool> DeleteCategoryAsync(Guid categoryId);

    Task<CategoryResponseDto?> GetCategoryByIdAsync(Guid categoryId);

    Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
}