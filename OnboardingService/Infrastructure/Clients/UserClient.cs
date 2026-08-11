using BuildingBlocks.Shared.Contracts.User.Request;

using BuildingBlocks.Shared.Contracts.User.Response;

using Onboarding.Application.Interfaces;
using System.Net.Http.Json;

namespace Onboarding.Infrastructure.Clients;

public sealed class UserClient : IUserClient
{
    private readonly HttpClient _httpClient;

    private const string UsersEndpoint = "api/internal/users";

    public UserClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<InternalCreateUserResponse> CreateUserAsync(
        InternalCreateUserRequest request,
        CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync(
            UsersEndpoint,
            request,
            ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);

            throw new HttpRequestException(
                $"User Service returned {(int)response.StatusCode}: {error}");
        }

        var body = await response.Content.ReadFromJsonAsync<InternalCreateUserResponse>(
            cancellationToken: ct)
            ?? throw new InvalidOperationException(
                "User Service returned an empty response.");

        if (body.UserId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "User Service returned an invalid UserId.");
        }

        return body;
    }

    public async Task DeleteUserAsync(
        Guid userId,
        CancellationToken ct)
    {
        var response = await _httpClient.DeleteAsync(
            $"{UsersEndpoint}/{userId}",
            ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);

            throw new HttpRequestException(
                $"User Service returned {(int)response.StatusCode}: {error}");
        }
    }
}