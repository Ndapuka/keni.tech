using MediatR;

namespace CompanyService.Application.Commands.CompleteContactInformation;

public sealed record CompleteContactInformationCommand : IRequest
{
    public Guid CompanyId { get; init; }

    public string Email { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;
}
