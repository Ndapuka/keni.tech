using CompanyService.Application.Interfaces;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyService.Application.Commands.UpdateCompany;

public sealed class UpdateCompanyCommandHandler
    : IRequestHandler<UpdateCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCompanyCommandHandler> _logger;

    public UpdateCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateCompanyCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(
        UpdateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(
            request.CompanyId,
            cancellationToken);

        if (company is null)
            throw new CompanyNotFoundException(request.CompanyId);

        company.UpdateName(request.Name);

        company.ChangeBusinessType(request.BusinessType);

        _companyRepository.Update(company);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Company {CompanyId} successfully updated.",
            company.Id);
    }
}