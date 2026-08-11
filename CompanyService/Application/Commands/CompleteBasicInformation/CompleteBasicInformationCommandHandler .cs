using CompanyService.Application.Interfaces;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyService.Application.Commands.CompleteBasicInformation;

public sealed class CompleteBasicInformationCommandHandler
    : IRequestHandler<CompleteBasicInformationCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompleteBasicInformationCommandHandler> _logger;

    public CompleteBasicInformationCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        ILogger<CompleteBasicInformationCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(
        CompleteBasicInformationCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(
            request.CompanyId,
            cancellationToken);

        if (company is null)
            throw new CompanyNotFoundException(request.CompanyId);

        company.CompleteBasicInformation(request.Slug);

        _companyRepository.Update(company);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Company {CompanyId} completed Basic Information step.",
            company.Id);
    }
}