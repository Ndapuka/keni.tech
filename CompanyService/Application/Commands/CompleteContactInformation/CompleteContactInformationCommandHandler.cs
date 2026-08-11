using CompanyService.Application.Interfaces;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Exceptions;
using CompanyService.Core.ValueObjects;
using Core.Constants;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyService.Application.Commands.CompleteContactInformation;

public sealed class CompleteContactInformationCommandHandler
    : IRequestHandler<CompleteContactInformationCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompleteContactInformationCommandHandler> _logger;

    public CompleteContactInformationCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        ILogger<CompleteContactInformationCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(
        CompleteContactInformationCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(
            request.CompanyId,
            cancellationToken);

        if (company is null)
            throw new CompanyNotFoundException(request.CompanyId);

        var contact = new Contact(
            request.Email,
            request.Phone);

        company.CompleteContactInformation(contact);

        _companyRepository.Update(company);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Company {CompanyId} completed Contact Information step.",
            company.Id);
    }
}
