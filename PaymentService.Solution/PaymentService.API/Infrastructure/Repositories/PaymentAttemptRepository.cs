
using PaymentService.Core.Entities;
using PaymentService.Core.Interfaces.Repositories;

namespace PaymentService.Infrastructure.Persistence.Repositories;

public sealed class PaymentAttemptRepository : IPaymentAttemptRepository
{
    private readonly PaymentDbContext _context;

    public PaymentAttemptRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        PaymentAttempt attempt,
        CancellationToken cancellationToken = default)
    {
        await _context.PaymentAttempts.AddAsync(
            attempt,
            cancellationToken);
    }
}