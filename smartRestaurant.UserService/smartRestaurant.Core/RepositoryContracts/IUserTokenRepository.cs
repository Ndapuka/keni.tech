using smartRestaurant.Core.Entities;


namespace smartRestaurant.Core.RepositoryContracts
{
    public interface IUserTokenRepository
    {
        Task<UserToken> CreateAsync(UserToken userToken);

        Task<UserToken?> GetByTokenAsync(string token);

        Task<UserToken?> GetActiveTokenAsync(Guid userId, TokenType tokenType);

        Task UpdateAsync(UserToken userToken);

        Task DeleteAsync(UserToken userToken);
    }
}
