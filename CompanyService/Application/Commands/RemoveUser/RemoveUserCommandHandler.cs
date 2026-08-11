using CompanyService.Application.Interfaces;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyService.Application.Commands.RemoveUser;

public sealed class RemoveUserCommandHandler
    : IRequestHandler<RemoveUserCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveUserCommandHandler> _logger;

    public RemoveUserCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        ILogger<RemoveUserCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(
        RemoveUserCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdForMemberAsync(
            request.CompanyId,
            request.RemovedByUserId,
            cancellationToken);

        if (company is null)
            throw new CompanyNotFoundException(request.CompanyId);

        company.EnsureIsOwner(request.RemovedByUserId);

        company.RemoveUser(request.UserId);

        _companyRepository.Update(company);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "User {UserId} deactivated in Company {CompanyId} by {RemovedByUserId}.",
            request.UserId,
            request.CompanyId,
            request.RemovedByUserId);
    }
}