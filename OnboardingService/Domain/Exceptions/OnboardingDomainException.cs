namespace Onboarding.Domain.Exceptions;

public abstract class OnboardingDomainException : Exception
{
    protected OnboardingDomainException(string message) : base(message) { }
    protected OnboardingDomainException(string message, Exception innerException) : base(message, innerException) { }
}
