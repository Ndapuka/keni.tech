using Microsoft.EntityFrameworkCore;
using smartRestaurant.Core.Entities;
using smartRestaurant.Core.RepositoryContracts;
using smartRestaurant.Infrastructure.Persistence;

namespace smartRestaurant.Infrastructure.Repositories;

public class UserTokenRepository : IUserTokenRepository
{
    private readonly ApplicationDbContext _context;

    public UserTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserToken> CreateAsync(UserToken userToken)
    {
        await _context.UserTokens.AddAsync(userToken);

        await _context.SaveChangesAsync();

        return userToken;
    }

    public async Task<UserToken?> GetByTokenAsync(string token)
    {
        return await _context.UserTokens
        .FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task<UserToken?> GetActiveTokenAsync(
        Guid userId,
        TokenType tokenType)
    {
        return await _context.UserTokens
            .FirstOrDefaultAsync(t =>
                t.UserId == userId &&
                t.TokenType == tokenType &&
                !t.IsUsed &&
                t.ExpiresAt > DateTime.UtcNow);
    }

    public async Task UpdateAsync(UserToken userToken)
    {
        _context.UserTokens.Update(userToken);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(UserToken userToken)
    {
        _context.UserTokens.Remove(userToken);

        await _context.SaveChangesAsync();
    }
}