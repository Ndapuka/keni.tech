using BuildingBlocks.Shared.Contracts.Enums;
using MediatR;

namespace CompanyService.Application.Commands.UpdateCompany;

public sealed record UpdateCompanyCommand : IRequest
{
    public Guid CompanyId { get; init; }

    /// <summary>
    /// Utilizador autenticado que pede a alteração — usado para
    /// validar membership e role (Owner/Admin) antes de mutar.
    /// </summary>
    public Guid UserId { get; init; }

    public string Name { get; init; } = string.Empty;

    public BusinessType BusinessType { get; init; }
}