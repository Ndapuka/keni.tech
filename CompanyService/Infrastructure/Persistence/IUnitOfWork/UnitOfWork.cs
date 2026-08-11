using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Infrastructure.Persistence.Context;

namespace CompanyService.Infrastructure.Persistence.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly CompanyDbContext _context;

    public UnitOfWork(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}