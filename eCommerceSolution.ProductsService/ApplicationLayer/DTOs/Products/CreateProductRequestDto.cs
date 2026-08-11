using System;
namespace ApplicationLayer.DTOs.Products;

public class CreateProductRequestDto
{
    public Guid CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal? PromotionalPrice { get; set; }

    public int PreparationTimeHours { get; set; }

    public int MinimumAdvanceHours { get; set; }

    public int MaximumDailyQuantity { get; set; }

    public bool StockControlled { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsCustomizable { get; set; }
}