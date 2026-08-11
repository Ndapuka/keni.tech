using BusinessLogicLayer.RepositoryContracts;
using DataAccessLayer.Context;

namespace DataAccessLayer.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProductsDbContext _context;

    public UnitOfWork(ProductsDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}