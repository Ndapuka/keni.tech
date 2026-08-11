
using Microsoft.EntityFrameworkCore;
using PaymentService.Core.Entities;
using PaymentService.Core.Interfaces.Repositories;

namespace PaymentService.Infrastructure.Persistence.Repositories;

public sealed class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;

    public PaymentRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Payment payment,
        CancellationToken cancellationToken = default)
    {
        await _context.Payments.AddAsync(payment, cancellationToken);
    }

    public async Task<Payment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Payment?> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);
    }

    public void Update(Payment payment)
    {
        _context.Payments.Update(payment);
    }
}