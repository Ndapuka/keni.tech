using AutoMapper;
using CompanyService.Application.DTOs.Responses;
using CompanyService.Application.Interfaces;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Exceptions;
using MediatR;

namespace CompanyService.Application.Queries.GetCompanyQuery;

public sealed class GetCompanyQueryHandler
    : IRequestHandler<GetCompanyQuery, CompanyResponse>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public GetCompanyQueryHandler(
        ICompanyRepository companyRepository,
        IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<CompanyResponse> Handle(
        GetCompanyQuery request,
        CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(
            request.CompanyId,
            cancellationToken);

        if (company is null)
            throw new CompanyNotFoundException(request.CompanyId);

        return _mapper.Map<CompanyResponse>(company);
    }
}