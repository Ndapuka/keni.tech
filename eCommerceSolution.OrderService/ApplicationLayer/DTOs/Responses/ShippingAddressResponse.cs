namespace ApplicationLayer.DTOs.Responses;

public class ShippingAddressResponse
{
    public Guid Id { get; set; }

    public string RecipientName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;
}