using AutoMapper;
using CompanyService.Application.DTOs.Responses;

using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Exceptions;
using MediatR;

namespace CompanyService.Application.Queries.GetCurrentCompany;

public sealed class GetCurrentCompanyQueryHandler
    : IRequestHandler<GetCurrentCompanyQuery, CompanyResponse>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public GetCurrentCompanyQueryHandler(
        ICompanyRepository companyRepository,
        IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    // GetByIdForMemberAsync valida a existência da empresa E a
    // pertença ativa do utilizador numa só chamada — se o token
    // ainda tiver a companyId mas o utilizador tiver sido removido
    // (CompanyUser.IsActive = false), isto falha aqui, não silenciosamente.
    public async Task<CompanyResponse> Handle(
     GetCurrentCompanyQuery request,
     CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdForMemberAsync(
            request.CompanyId,
            request.UserId,
            cancellationToken);

        if (company is null)
            throw new CompanyNotFoundException(request.CompanyId);

        return _mapper.Map<CompanyResponse>(company);
    }
}