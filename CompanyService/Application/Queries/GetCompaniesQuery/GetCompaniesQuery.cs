using CompanyService.Application.DTOs.Responses;
using MediatR;

namespace CompanyService.Application.Queries.GetCompaniesQuery;

public sealed record GetCompaniesQuery(Guid UserId)
    : IRequest<IReadOnlyCollection<CompanyResponse>>;