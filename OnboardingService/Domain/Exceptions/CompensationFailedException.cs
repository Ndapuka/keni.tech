namespace Onboarding.Domain.Exceptions;

public sealed class CompensationFailedException : OnboardingDomainException
{
    public Guid OrphanedUserId { get; }

    public CompensationFailedException(
        Guid orphanedUserId,
        string message)
        : base(message)
    {
        OrphanedUserId = orphanedUserId;
    }

    public CompensationFailedException(
        Guid orphanedUserId,
        string message,
        Exception innerException)
        : base(message, innerException)
    {
        OrphanedUserId = orphanedUserId;
    }
}