namespace BuildingBlocks.Shared.Contracts.User.Request;

public class InternalCreateUserRequest
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public string PersonName { get; set; } = default!;

    public string? PhoneNumber { get; set; }
}
