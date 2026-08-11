using BusinessLogicLayer.Enums;
namespace BusinessLogicLayer.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public Guid CategoryId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public decimal? PromotionalPrice { get; private set; }

    public int PreparationTimeHours { get; private set; }

    public int MinimumAdvanceHours { get; private set; }

    public int MaximumDailyQuantity { get; private set; }

    public bool StockControlled { get; private set; }

    public bool IsAvailable { get; private set; }

    public bool IsFeatured { get; private set; }

    public ProductStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    // Navigation

    public Category Category { get; private set; } = null!;

    public ICollection<ProductImage> Images { get; private set; } = new List<ProductImage>();

    protected Product()
    {

    }

    public Product(
        Guid categoryId,
        string name,
        string description,
        decimal price,
        int preparationTimeHours,
        int minimumAdvanceHours,
        int maximumDailyQuantity,
        bool stockControlled)
    {
        Id = Guid.NewGuid();

        CategoryId = categoryId;

        Name = name;

        Description = description;

        Price = price;

        PreparationTimeHours = preparationTimeHours;

        MinimumAdvanceHours = minimumAdvanceHours;

        MaximumDailyQuantity = maximumDailyQuantity;

        StockControlled = stockControlled;

        IsAvailable = true;

        IsFeatured = false;

        Status = ProductStatus.Available;

        CreatedAt = DateTime.UtcNow;

        UpdatedAt = DateTime.UtcNow;
    }
}