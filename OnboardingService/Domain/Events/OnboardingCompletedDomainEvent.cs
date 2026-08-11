namespace Onboarding.Domain.Events;

// Evento interno ao processo (não é mensageria/RabbitMQ) — usado para desacoplar
// o handler principal de side-effects como logging estruturado ou métricas.
public sealed record OnboardingCompletedDomainEvent(
    Guid UserId,
    Guid CompanyId,
    DateTime CompletedAtUtc
);