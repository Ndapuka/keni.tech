
using BuildingBlocks.Shared.Contracts.User.Request;
using BuildingBlocks.Shared.Contracts.User.Response;



namespace Onboarding.Application.Interfaces;

public interface IUserClient
{
    Task<InternalCreateUserResponse> CreateUserAsync(
        InternalCreateUserRequest request,
        CancellationToken ct);

    Task DeleteUserAsync(
        Guid userId,
        CancellationToken ct);
}