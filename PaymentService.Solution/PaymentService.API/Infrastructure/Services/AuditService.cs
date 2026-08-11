using Microsoft.AspNetCore.Http;
using PaymentService.Application.ServiceContracts;
using PaymentService.Core.Entities;
using PaymentService.Core.Interfaces.Common;
using PaymentService.Core.Interfaces.Repositories;

namespace PaymentService.Infrastructure.Services;

public sealed class AuditService : IAuditService
{
    private readonly IPaymentAuditRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditService(
        IPaymentAuditRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogAsync(
        PaymentAudit audit,
        CancellationToken cancellationToken = default)
    {
        await _repository.AddAsync(audit, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}