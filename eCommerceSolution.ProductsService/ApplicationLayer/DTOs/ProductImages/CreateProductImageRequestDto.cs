namespace ApplicationLayer.DTOs.ProductImages;

public class CreateProductImageRequestDto
{
    public Guid ProductId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}