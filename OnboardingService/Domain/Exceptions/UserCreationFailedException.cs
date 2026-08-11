namespace Onboarding.Domain.Exceptions;

public class UserCreationFailedException : OnboardingDomainException
{
    public UserCreationFailedException(string reason)
        : base($"Failed to create user during onboarding: {reason}") { }

    public UserCreationFailedException(string reason, Exception innerException)
        : base($"Failed to create user during onboarding: {reason}", innerException) { }
}
