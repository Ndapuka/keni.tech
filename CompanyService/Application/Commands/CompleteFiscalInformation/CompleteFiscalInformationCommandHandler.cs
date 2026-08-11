using CompanyService.Application.Interfaces;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Exceptions;
using CompanyService.Core.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyService.Application.Commands.CompleteFiscalInformation;

public sealed class CompleteFiscalInformationCommandHandler
    : IRequestHandler<CompleteFiscalInformationCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompleteFiscalInformationCommandHandler> _logger;

    public CompleteFiscalInformationCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        ILogger<CompleteFiscalInformationCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(
        CompleteFiscalInformationCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(
            request.CompanyId,
            cancellationToken);

        if (company is null)
            throw new CompanyNotFoundException(request.CompanyId);

        var address = new Address(
            request.Street,
            request.City,
            request.PostalCode,
            request.Country);

        company.CompleteFiscalInformation(
            request.TaxNumber,
            address);

        _companyRepository.Update(company);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Company {CompanyId} completed Fiscal Information step.",
            company.Id);
    }
}