using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Entities;
using CompanyService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CompanyService.Infrastructure.Persistence.Repositories;

public sealed class CompanyRepository
    : Repository<Company>, ICompanyRepository
{
    private readonly CompanyDbContext _context;

    public CompanyRepository(CompanyDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<Company?> GetByIdAsync(
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .Include(c => c.Users)
            .FirstOrDefaultAsync(
                c => c.Id == companyId,
                cancellationToken);
    }

    public async Task<bool> ExistsBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .AnyAsync(
                c => c.Slug == slug,
                cancellationToken);
    }

    public async Task<bool> ExistsByTaxNumberAsync(
        string taxNumber,
        CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .AnyAsync(
                c => c.TaxNumber == taxNumber,
                cancellationToken);
    }

    public async Task<bool> ExistsByOwnerAsync(
        Guid ownerUserId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .AnyAsync(
                c => c.OwnerUserId == ownerUserId,
                cancellationToken);
    }

    public async Task<Company?> GetByOwnerUserIdAsync(
        Guid ownerUserId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .Include(c => c.Users)
            .FirstOrDefaultAsync(
                c => c.OwnerUserId == ownerUserId,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<Company>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Devolve todas as empresas às quais o utilizador pertence
    /// através de uma membership ativa.
    /// </summary>
    public async Task<IReadOnlyCollection<Company>> GetByMemberUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .AsNoTracking()
            .Where(c => c.Users.Any(
                u => u.UserId == userId &&
                     u.IsActive))
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Devolve a empresa apenas quando o utilizador é
    /// membro ativo dessa empresa.
    /// </summary>
    public async Task<Company?> GetByIdForMemberAsync(
        Guid companyId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.Id == companyId &&
                     c.Users.Any(
                         u => u.UserId == userId &&
                              u.IsActive),
                cancellationToken);
    }
}