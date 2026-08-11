using MediatR;

namespace CompanyService.Application.Commands.CompleteBasicInformation;

public sealed record CompleteBasicInformationCommand : IRequest
{
    public Guid CompanyId { get; init; }

    public string Slug { get; init; } = string.Empty;
}