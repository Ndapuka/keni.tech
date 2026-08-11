using BuildingBlocks.Shared.Contracts.Enums;


namespace Application.DTOs.testes;

public class CreateCompanyRequest
{
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string TaxNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string? LogoUrl { get; set; }

    public BusinessType BusinessType { get; set; }
}