using Microsoft.Extensions.Logging;
using Onboarding.Application.Interfaces;
using Onboarding.Domain.Exceptions;

namespace Onboarding.Application.Compensation;

public class CompensationService : ICompensationService
{
    private readonly IUserClient _userClient;
    private readonly ILogger<CompensationService> _logger;

    public CompensationService(
        IUserClient userClient,
        ILogger<CompensationService> logger)
    {
        _userClient = userClient;
        _logger = logger;
    }

    public async Task CompensateUserCreationAsync(
        Guid userId,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            exception,
            "Starting compensation for UserId {UserId}.",
            userId);

        try
        {
            await _userClient.DeleteUserAsync(userId, cancellationToken);

            _logger.LogInformation(
                "Compensation completed successfully for UserId {UserId}.",
                userId);
        }
        catch (Exception compensationException)
        {
            _logger.LogCritical(
                compensationException,
                "Compensation failed for UserId {UserId}. Manual intervention required.",
                userId);

            throw new CompensationFailedException(
                userId,
                compensationException.Message,
                compensationException);
        }
    }
}