namespace PaymentService.Core.Interfaces.Common;

public interface ICurrentUserService
{
    string? UserId { get; }

    string? UserName { get; }
}