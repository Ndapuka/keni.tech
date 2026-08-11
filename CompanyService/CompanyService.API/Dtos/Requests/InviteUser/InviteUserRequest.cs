using BuildingBlocks.Shared.Contracts.Enums;

namespace CompanyService.API.Dtos.Requests.InviteUser;

public sealed record InviteUserRequest
{
    public Guid UserId { get; init; }

    public CompanyRole Role { get; init; }
}