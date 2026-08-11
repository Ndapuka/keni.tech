using CompanyService.Application.DTOs.Responses;
using MediatR;

namespace CompanyService.Application.Queries.GetCompanyDashboard;

public sealed record GetCompanyDashboardQuery(
    Guid CompanyId,
    Guid UserId)
    : IRequest<CompanyDashboardResponse>;