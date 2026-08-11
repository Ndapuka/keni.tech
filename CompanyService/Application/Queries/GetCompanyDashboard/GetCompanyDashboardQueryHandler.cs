using AutoMapper;
using CompanyService.Application.DTOs.Responses;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Exceptions;
using MediatR;

namespace CompanyService.Application.Queries.GetCompanyDashboard;

public sealed class GetCompanyDashboardQueryHandler
    : IRequestHandler<GetCompanyDashboardQuery, CompanyDashboardResponse>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public GetCompanyDashboardQueryHandler(
        ICompanyRepository companyRepository,
        IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<CompanyDashboardResponse> Handle(GetCompanyDashboardQuery request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdForMemberAsync(
            request.CompanyId,
            request.UserId,
            cancellationToken);

        if (company is null)
            throw new CompanyNotFoundException(request.CompanyId);

        return _mapper.Map<CompanyDashboardResponse>(company);
    }
}