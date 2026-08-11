namespace smartRestaurant.Core.Entities;

public class UserToken
{
    public Guid UserTokenId { get; set; }

    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = default!;

    public string Token { get; set; } = default!;

    public TokenType TokenType { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }

    public string? EmailConfirmationToken { get; set; }

    public DateTime? EmailConfirmationTokenExpiresAt { get; set; }
}
