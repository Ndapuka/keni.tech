using Microsoft.Extensions.Logging;
using Onboarding.Application.Idempotency;
using System.Collections.Concurrent;

namespace Onboarding.Infrastructure.Idempotency;

/// <summary>
/// Implementação em memória da Idempotência.
/// Apenas para desenvolvimento ou single instance.
/// Em produção será substituída por Redis.
/// </summary>
public sealed class InMemoryIdempotencyStore : IIdempotencyStore
{
    private readonly ConcurrentDictionary<Guid, RegisterCompanyResponseCache> _store = new();

    private readonly ILogger<InMemoryIdempotencyStore> _logger;

    public InMemoryIdempotencyStore(
        ILogger<InMemoryIdempotencyStore> logger)
    {
        _logger = logger;
    }

    public Task<RegisterCompanyResponseCache?> TryGetAsync(
        Guid idempotencyKey,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Checking idempotency. StoreCount={StoreCount}, Key={Key}",
            _store.Count,
            idempotencyKey);

        _store.TryGetValue(idempotencyKey, out var value);

        _logger.LogInformation(
            "Idempotency result: {Result}",
            value is null ? "MISS" : "HIT");

        return Task.FromResult(value);
    }

    public Task SaveAsync(
        Guid idempotencyKey,
        RegisterCompanyResponseCache result,
        CancellationToken ct)
    {
        _store[idempotencyKey] = result;

        _logger.LogInformation(
            "Idempotency saved. Key={Key}, StoreCount={StoreCount}",
            idempotencyKey,
            _store.Count);

        return Task.CompletedTask;
    }
}