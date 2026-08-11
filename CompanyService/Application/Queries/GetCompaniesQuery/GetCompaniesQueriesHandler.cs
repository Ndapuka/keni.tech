using AutoMapper;
using CompanyService.Application.DTOs.Responses;
using CompanyService.Application.Interfaces.Persistence;
using MediatR;

namespace CompanyService.Application.Queries.GetCompaniesQuery;

public sealed class GetCompaniesQueryHandler
    : IRequestHandler<
        GetCompaniesQuery,
        IReadOnlyCollection<CompanyResponse>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public GetCompaniesQueryHandler(
        ICompanyRepository companyRepository,
        IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<CompanyResponse>> Handle(
        GetCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var companies =
            await _companyRepository.GetByMemberUserIdAsync(
                request.UserId,
                cancellationToken);

        return _mapper.Map<IReadOnlyCollection<CompanyResponse>>(companies);
    }
}