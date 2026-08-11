using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs.Requests;

public class ShippingAddressRequest
{
    [Required]
    public string RecipientName { get; set; } = string.Empty;

    [Required]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string PostalCode { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;
}
