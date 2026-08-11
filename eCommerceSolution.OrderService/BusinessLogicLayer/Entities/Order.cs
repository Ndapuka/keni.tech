using BusinessLogicLayer.Enums;
namespace BusinessLogicLayer.Entities;

public class Order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public decimal SubTotal { get; set; }

    public decimal Discount { get; set; }

    public decimal ShippingCost { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Notes { get; set; }

    public ShippingAddress? ShippingAddress { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}



