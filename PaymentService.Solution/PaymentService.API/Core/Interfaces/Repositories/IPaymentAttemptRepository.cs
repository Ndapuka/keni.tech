using PaymentService.Core.Entities;

namespace PaymentService.Core.Interfaces.Repositories;

public interface IPaymentAttemptRepository
{
    Task AddAsync(PaymentAttempt attempt, CancellationToken cancellationToken = default);
}