using CompanyService.Application.Interfaces;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyService.Application.Commands.InviteUser;

public sealed class InviteUserCommandHandler
    : IRequestHandler<InviteUserCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<InviteUserCommandHandler> _logger;

    public InviteUserCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        ILogger<InviteUserCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Guid> Handle(
        InviteUserCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdForMemberAsync(
            request.CompanyId,
            request.InvitedByUserId,
            cancellationToken);

        if (company is null)
            throw new CompanyNotFoundException(request.CompanyId);

        company.EnsureCanManage(request.InvitedByUserId);

        company.InviteUser(
            request.UserId,
            request.Role);

        _companyRepository.Update(company);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "User {UserId} invited to Company {CompanyId} with role {Role} by {InvitedByUserId}.",
            request.UserId,
            request.CompanyId,
            request.Role,
            request.InvitedByUserId);

        return request.UserId;
    }
}