using CompanyService.Application.Interfaces.Persistence;
using MediatR;

namespace CompanyService.Application.Queries.CheckCompanyMembership;

public sealed class CheckCompanyMembershipQueryHandler
    : IRequestHandler<CheckCompanyMembershipQuery, bool>
{
    private readonly ICompanyRepository _companyRepository;

    public CheckCompanyMembershipQueryHandler(
        ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<bool> Handle(
        CheckCompanyMembershipQuery request,
        CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdForMemberAsync(
            request.CompanyId,
            request.UserId,
            cancellationToken);

        return company is not null;
    }
}
