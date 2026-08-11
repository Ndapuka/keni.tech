namespace ApplicationLayer.DTOs.External;

public class ProductResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }
}
