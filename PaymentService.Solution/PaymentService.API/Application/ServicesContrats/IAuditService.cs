using PaymentService.Core.Entities;

namespace PaymentService.Application.ServiceContracts;

public interface IAuditService
{
    Task LogAsync(
        PaymentAudit audit,
        CancellationToken cancellationToken = default);
}