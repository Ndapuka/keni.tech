using BuildingBlocks.Shared.Contracts.Enums;

namespace CompanyService.API.Dtos.Requests.RegisterCompany;

public sealed class RegisterCompanyRequest
{
    public Guid OwnerUserId { get; init; }

    public string Name { get; init; } = string.Empty;

    public BusinessType BusinessType { get; init; }

    public string Country { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;
}