using MediatR;

namespace CompanyService.Application.Commands.CompleteBranding;

public sealed record CompleteBrandingCommand : IRequest
{
    public Guid CompanyId { get; init; }

    public string? Description { get; init; }

    public string? LogoUrl { get; init; }
}