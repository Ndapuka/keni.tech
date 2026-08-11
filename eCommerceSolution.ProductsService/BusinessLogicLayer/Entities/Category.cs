namespace BusinessLogicLayer.Entities;

public class Category
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string? ImageUrl { get; private set; }

    public int DisplayOrder { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    // Navigation Property

    public ICollection<Product> Products { get; private set; } = new List<Product>();

    protected Category()
    {

    }

    public Category(
        string name,
        string description,
        string? imageUrl,
        int displayOrder)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        DisplayOrder = displayOrder;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
