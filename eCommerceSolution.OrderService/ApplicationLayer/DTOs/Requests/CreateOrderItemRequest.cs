
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs.Requests;

public class CreateOrderItemRequest
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public string ProductName { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}