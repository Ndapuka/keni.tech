using MediatR;

namespace CompanyService.Application.Commands.RemoveUser;

public sealed record RemoveUserCommand : IRequest
{
    public Guid CompanyId { get; init; }

    /// <summary>
    /// Utilizador autenticado que pede a remoção — para validar
    /// membership/role, e para registo de auditoria (quem desativou quem).
    /// </summary>
    public Guid RemovedByUserId { get; init; }

    public Guid UserId { get; init; }
}