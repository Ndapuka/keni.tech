namespace Onboarding.Contracts.Responses;

public class OnboardingErrorResponse
{
    public string Message { get; set; } = default!;
    public string Stage { get; set; } = default!; // "UserCreation" | "CompanyCreation" | "Compensation"
}
