namespace BusinessLogicLayer.Entities;

public class ProductImage
{
    public Guid Id { get; private set; }

    public Guid ProductId { get; private set; }

    public string ImageUrl { get; private set; } = string.Empty;

    public int DisplayOrder { get; private set; }

    public Product Product { get; private set; } = null!;

    protected ProductImage()
    {

    }

    public ProductImage(
        Guid productId,
        string imageUrl,
        int displayOrder)
    {
        Id = Guid.NewGuid();

        ProductId = productId;

        ImageUrl = imageUrl;

        DisplayOrder = displayOrder;
    }
}