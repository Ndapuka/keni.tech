namespace CompanyService.API.Dtos.Requests.CompleteFiscalInformation;

public sealed class CompleteFiscalInformationRequest
{
    public Guid CompanyId { get; init; }

    public string TaxNumber { get; init; } = string.Empty;

    public string Street { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string PostalCode { get; init; } = string.Empty;

    public string Country { get; init; } = string.Empty;
}