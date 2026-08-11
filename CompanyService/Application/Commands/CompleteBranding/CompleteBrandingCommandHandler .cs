using CompanyService.Application.Interfaces;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyService.Application.Commands.CompleteBranding;

public sealed class CompleteBrandingCommandHandler
    : IRequestHandler<CompleteBrandingCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompleteBrandingCommandHandler> _logger;

    public CompleteBrandingCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        ILogger<CompleteBrandingCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(
        CompleteBrandingCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(
            request.CompanyId,
            cancellationToken);

        if (company is null)
            throw new CompanyNotFoundException(request.CompanyId);

        company.CompleteBranding(
            request.Description,
            request.LogoUrl);

        _companyRepository.Update(company);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Company {CompanyId} completed Branding step.",
            company.Id);
    }
}