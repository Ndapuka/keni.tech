using ApplicationLayer.DTOs.External;

namespace ApplicationLayer.HttpClientsContracts;

public interface IUsersServiceClient
{
    Task<UserResponse?> GetUserByIdAsync(Guid userId);
}
