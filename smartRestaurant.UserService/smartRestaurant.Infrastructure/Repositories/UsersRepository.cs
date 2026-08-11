using smartRestaurant.Core.DTO;
using smartRestaurant.Core.Entities;
using smartRestaurant.Core.RepositoryContracts;
using smartRestaurant.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace smartRestaurant.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly ApplicationDbContext _context;
    public UsersRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(ApplicationUser user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task ConfirmEmailAsync(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);

        if (user is null)
            return;

        user.EmailConfirmed = true;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeactivateAsync(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);

        if (user is null)
            return;

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<ApplicationUser?> GetByIdAsync(Guid id)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserID == id);
    }

    public async Task<ApplicationUser?> GetForLoginAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email); //&& u.IsActive);
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdateAsync(ApplicationUser user)
    {
        _context.Users.Update(user);
    }
    public async Task<bool> ExistsUserNameAsync(string userName)
    {
        return await _context.Users.AnyAsync(u => u.UserName == userName);
    }
    public async Task<ApplicationUser?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await (_context.Users.AsNoTracking()
            .AnyAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken)
            );
    }
}

