namespace BusinessLogicLayer.Entities;

public class ShippingAddress
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public string RecipientName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public Order Order { get; set; } = null!;
}