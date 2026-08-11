using Microsoft.EntityFrameworkCore;
using PaymentService.Core.Entities;

namespace PaymentService.Infrastructure.Persistence;

public sealed class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options)
    {
    }

    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<PaymentAttempt> PaymentAttempts => Set<PaymentAttempt>();

    public DbSet<PaymentAudit> PaymentAudits => Set<PaymentAudit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
    }
}
