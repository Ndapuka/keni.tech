using MediatR;

namespace CompanyService.Application.Queries.CheckCompanyMembership;

public sealed record CheckCompanyMembershipQuery(
    Guid CompanyId,
    Guid UserId
) : IRequest<bool>;
