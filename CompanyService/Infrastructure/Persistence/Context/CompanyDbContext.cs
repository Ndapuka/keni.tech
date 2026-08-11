using BuildingBlocks.Shared.Contracts.Company.Common;
using CompanyService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyService.Infrastructure.Persistence.Context;

public sealed class CompanyDbContext : DbContext
{
    public CompanyDbContext(
        DbContextOptions<CompanyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<CompanyUser> CompanyUsers => Set<CompanyUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CompanyDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(x => x.CreatedAt).CurrentValue = utcNow;
                    break;

                case EntityState.Modified:
                    entry.Property(x => x.UpdatedAt).CurrentValue = utcNow;
                    break;
            }
        }
    }
}