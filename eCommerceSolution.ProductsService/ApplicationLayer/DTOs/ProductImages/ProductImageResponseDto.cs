namespace ApplicationLayer.DTOs.ProductImages;

public class ProductImageResponseDto
{
    public Guid Id { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}