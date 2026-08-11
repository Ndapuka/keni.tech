using System.Net.Http.Json;
using ApplicationLayer.DTOs.External;
using ApplicationLayer.HttpClientsContracts;

namespace ApplicationLayer.HttpClients;

public class UsersServiceClient : IUsersServiceClient
{
    private readonly HttpClient _httpClient;

    public UsersServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid userId)
    {
        return await _httpClient.GetFromJsonAsync<UserResponse>(
            $"api/users/{userId}");
    }
}