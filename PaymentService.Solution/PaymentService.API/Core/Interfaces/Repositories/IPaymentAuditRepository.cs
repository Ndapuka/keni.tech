using PaymentService.Core.Entities;

namespace PaymentService.Core.Interfaces.Repositories;

public interface IPaymentAuditRepository
{
    Task AddAsync(PaymentAudit audit, CancellationToken cancellationToken = default);
}