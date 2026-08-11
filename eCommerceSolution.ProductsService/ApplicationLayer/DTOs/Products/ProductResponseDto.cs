

using ApplicationLayer.DTOs.ProductImages;

namespace ApplicationLayer.DTOs.Products;

public class ProductResponseDto
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal? PromotionalPrice { get; set; }

    public int PreparationTimeHours { get; set; }

    public int MinimumAdvanceHours { get; set; }

    public int MaximumDailyQuantity { get; set; }

    public bool IsAvailable { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsCustomizable { get; set; }

    public string Status { get; set; } = string.Empty;

    public List<ProductImageResponseDto> Images { get; set; } = [];
}
