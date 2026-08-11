using BuildingBlocks.Shared.Contracts.Enums;


namespace BuildingBlocks.Shared.Contracts.Company.Requests;

public sealed class InternalCreateCompanyRequest
{
    public Guid OwnerUserId { get; init; }

    public string Name { get; init; } = string.Empty;

    public BusinessType BusinessType { get; init; }

    public string Country { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;
}
