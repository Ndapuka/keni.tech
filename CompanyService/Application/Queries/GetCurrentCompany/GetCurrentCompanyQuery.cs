using CompanyService.Application.DTOs.Responses;
using MediatR;

namespace CompanyService.Application.Queries.GetCurrentCompany;

/// <summary>
/// Resolve a empresa ativa do utilizador (companyId embutido no JWT).
/// Não confunde com "empresa cujo o utilizador é owner" — num modelo
/// multi-empresa, o utilizador pode ter várias, ativa é só a selecionada.
/// </summary>
public sealed record GetCurrentCompanyQuery(Guid CompanyId, Guid UserId)
    : IRequest<CompanyResponse>;
