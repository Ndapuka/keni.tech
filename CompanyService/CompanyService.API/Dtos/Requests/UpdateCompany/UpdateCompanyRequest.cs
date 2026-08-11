using BuildingBlocks.Shared.Contracts.Enums;

namespace CompanyService.API.Dtos.Requests.UpdateCompany;

public sealed class UpdateCompanyRequest
{
    public Guid CompanyId { get; init; }

    public string Name { get; init; } = string.Empty;

    public BusinessType BusinessType { get; init; }
}