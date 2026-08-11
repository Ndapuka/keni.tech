using BuildingBlocks.Shared.Contracts.Company.Common;
using BuildingBlocks.Shared.Contracts.Enums;
using MediatR;

namespace CompanyService.Application.Commands.InviteUser;

public sealed record InviteUserCommand : IRequest<Guid>
{
    public Guid CompanyId { get; init; }

    /// <summary>
    /// Utilizador autenticado que está a convidar — usado para
    /// validar membership e role (Owner/Admin) antes de mutar.
    /// </summary>
    public Guid InvitedByUserId { get; init; }

    public Guid UserId { get; init; }

    public CompanyRole Role { get; init; }
}