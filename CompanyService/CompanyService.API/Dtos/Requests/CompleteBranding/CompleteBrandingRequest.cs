namespace CompanyService.API.Dtos.Requests.CompleteBranding;

public sealed class CompleteBrandingRequest
{
    public Guid CompanyId { get; init; }

    public string? Description { get; init; }

    public string? LogoUrl { get; init; }
}