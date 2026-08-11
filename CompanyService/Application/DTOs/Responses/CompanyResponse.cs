using BuildingBlocks.Shared.Contracts.Enums;

namespace CompanyService.Application.DTOs.Responses;

public sealed class CompanyResponse
{
    public Guid CompanyId { get; init; }

    public string Name { get; init; } = string.Empty;

    public BusinessType BusinessType { get; init; }

    public string Status { get; init; } = string.Empty;

    public string WizardStep { get; init; } = string.Empty;

    public string Country { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;
}