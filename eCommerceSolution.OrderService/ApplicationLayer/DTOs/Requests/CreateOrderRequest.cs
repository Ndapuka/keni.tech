using ApplicationLayer.DTOs.Requests;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs.Requests;

public class CreateOrderRequest
{
    [Required]
    public Guid UserId { get; set; }

    public string? Notes { get; set; }

    [Required]
    public ShippingAddressRequest ShippingAddress { get; set; } = null!;

    [Required]
    public List<CreateOrderItemRequest> OrderItems { get; set; } = new();
}



