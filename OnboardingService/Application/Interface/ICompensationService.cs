namespace Onboarding.Application.Interfaces;

public interface ICompensationService
{
    Task CompensateUserCreationAsync(
        Guid userId,
        Exception exception,
        CancellationToken cancellationToken);
}