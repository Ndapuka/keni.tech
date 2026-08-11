using CompanyService.Application.DTOs.Responses;
using MediatR;

namespace CompanyService.Application.Queries.GetCompanyQuery;

public sealed record GetCompanyQuery(Guid CompanyId)
    : IRequest<CompanyResponse>;