using CompanyService.Application.DTOs.Responses;
using CompanyService.Application.Interfaces;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyService.Application.Commands.RegisterCompany;

public sealed class RegisterCompanyCommandHandler
    : IRequestHandler<RegisterCompanyCommand, RegisterCompanyResponse>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterCompanyCommandHandler> _logger;

    public RegisterCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        ILogger<RegisterCompanyCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<RegisterCompanyResponse> Handle(
    RegisterCompanyCommand request,
    CancellationToken cancellationToken)
    {
        var company = Company.Register(
            request.OwnerUserId,
            request.Name,
            request.BusinessType,
            request.Country,
            request.City);

        await _companyRepository.AddAsync(
            company,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        _logger.LogInformation(
            "Company {CompanyId} successfully registered.",
            company.Id);

        return new RegisterCompanyResponse
        {
            CompanyId = company.Id,
            Status = company.Status.ToString(),
            WizardStep = company.WizardStep.ToString()
        };
    }
}
