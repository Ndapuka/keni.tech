using PaymentService.Core.Entities;
using PaymentService.Core.Interfaces.Repositories;

namespace PaymentService.Infrastructure.Persistence.Repositories;

public sealed class PaymentAuditRepository : IPaymentAuditRepository
{
    private readonly PaymentDbContext _context;

    public PaymentAuditRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        PaymentAudit audit,
        CancellationToken cancellationToken = default)
    {
        await _context.PaymentAudits.AddAsync(
            audit,
            cancellationToken);
    }
}
