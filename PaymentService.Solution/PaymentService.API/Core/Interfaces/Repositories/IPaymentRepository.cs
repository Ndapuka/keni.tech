using PaymentService.Core.Entities;

namespace PaymentService.Core.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);

    void Update(Payment payment);
}