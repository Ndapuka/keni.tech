namespace Onboarding.Domain.Exceptions;

public class CompanyCreationFailedException : OnboardingDomainException
{
    public Guid UserId { get; }

    public CompanyCreationFailedException(Guid userId, string reason)
        : base($"Failed to create company for user {userId}: {reason}")
    {
        UserId = userId;
    }

    public CompanyCreationFailedException(Guid userId, string reason, Exception innerException)
        : base($"Failed to create company for user {userId}: {reason}", innerException)
    {
        UserId = userId;
    }
}