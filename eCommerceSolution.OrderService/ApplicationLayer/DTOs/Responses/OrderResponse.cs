using ApplicationLayer.DTOs.Responses;
using BusinessLogicLayer.Enums;

namespace ApplicationLayer.DTOs.Responses;

public class OrderResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public OrderStatus Status { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Discount { get; set; }

    public decimal ShippingCost { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Notes { get; set; }

    public ShippingAddressResponse ShippingAddress { get; set; } = null!;

    public List<OrderItemResponse> OrderItems { get; set; } = new();
}
