namespace Onboarding.Application.Idempotency;

// Abstração simples: implementação em memória agora, Redis depois — sem tocar na Application.
public interface IIdempotencyStore
{
    Task<RegisterCompanyResponseCache?> TryGetAsync(Guid idempotencyKey, CancellationToken ct);
    Task SaveAsync(Guid idempotencyKey, RegisterCompanyResponseCache result, CancellationToken ct);
}

public record RegisterCompanyResponseCache(Guid UserId, Guid CompanyId);