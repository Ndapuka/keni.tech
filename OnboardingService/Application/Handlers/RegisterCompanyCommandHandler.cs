using BuildingBlocks.Shared.Contracts.Company.Requests;
using BuildingBlocks.Shared.Contracts.User.Request;
using MediatR;
using Microsoft.Extensions.Logging;
using Onboarding.Application.Commands;
using Onboarding.Application.Idempotency;
using Onboarding.Application.Interfaces;
using Onboarding.Contracts.Responses;
using Onboarding.Domain.Enums;
using Onboarding.Domain.Exceptions;

namespace Onboarding.Application.Handlers;

public class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand, RegisterCompanyResponse>
{
    private readonly IUserClient _userClient;
    private readonly ICompanyClient _companyClient;
    private readonly IIdempotencyStore _idempotencyStore;
    private readonly ICompensationService _compensationService;
    private readonly ILogger<RegisterCompanyCommandHandler> _logger;

    public RegisterCompanyCommandHandler(
        IUserClient userClient,
        ICompanyClient companyClient,
        IIdempotencyStore idempotencyStore,
        ICompensationService compensationService,
        ILogger<RegisterCompanyCommandHandler> logger)
    {
        _userClient = userClient;
        _companyClient = companyClient;
        _idempotencyStore = idempotencyStore;
        _compensationService = compensationService;
        _logger = logger;
    }

    public async Task<RegisterCompanyResponse> Handle(
    RegisterCompanyCommand request,
    CancellationToken ct)
    {
        using var _ = BeginLoggingScope(request);

        var cached = await TryGetCachedResponseAsync(request, ct);
        if (cached is not null)
            return cached;

        _logger.LogInformation("Starting onboarding workflow.");

        var userId = await CreateUserAsync(request, ct);

        var companyId = await CreateCompanyAsync(request, userId, ct);

        await SaveIdempotencyAsync(
            request.IdempotencyKey,
            userId,
            companyId,
            ct);

        _logger.LogInformation(
            "Onboarding completed successfully. UserId {UserId}, CompanyId {CompanyId}.",
            userId,
            companyId);

        return BuildResponse(userId, companyId);
    }

    private IDisposable BeginLoggingScope(RegisterCompanyCommand request)
    {
        return _logger.BeginScope(new Dictionary<string, object>
        {
            ["IdempotencyKey"] = request.IdempotencyKey,
            ["Email"] = request.Email,
            ["CompanyName"] = request.CompanyName
        })!;
    }


    private async Task<RegisterCompanyResponse?> TryGetCachedResponseAsync(
    RegisterCompanyCommand request,
    CancellationToken ct)
    {
        var cached = await _idempotencyStore.TryGetAsync(
            request.IdempotencyKey,
            ct);

        if (cached is null)
            return null;

        _logger.LogInformation(
            "Idempotent replay detected. Returning cached onboarding result for UserId {UserId}, CompanyId {CompanyId}.",
            cached.UserId,
            cached.CompanyId);
        _logger.LogInformation(" MOISES Checking idempotency for key {Key}", request.IdempotencyKey);

        return BuildResponse(
            cached.UserId,
            cached.CompanyId);
    }


    private async Task<Guid> CreateUserAsync(
    RegisterCompanyCommand request,
    CancellationToken ct)
    {
        try
        {
            var result = await _userClient.CreateUserAsync(
                BuildUserPayload(request),
                ct);

            _logger.LogInformation(
                "Onboarding stage UserCreation succeeded. UserId {UserId}.",
                result.UserId);

            return result.UserId;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Onboarding stage UserCreation failed for email {Email}.",
                request.Email);

            throw new UserCreationFailedException(
                ex.Message,
                ex);
        }
    }


    private async Task<Guid> CreateCompanyAsync(
    RegisterCompanyCommand request,
    Guid userId,
    CancellationToken ct)
    {
        try
        {
            var result = await _companyClient.CreateCompanyAsync(
                BuildCompanyPayload(request, userId),
                ct);

            _logger.LogInformation(
                "Onboarding stage CompanyCreation succeeded. UserId {UserId}, CompanyId {CompanyId}.",
                userId,
                result.CompanyId);

            return result.CompanyId;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Onboarding stage CompanyCreation failed for UserId {UserId}. Initiating compensation.",
                userId);

            await _compensationService.CompensateUserCreationAsync(
                userId,
                ex,
                ct);

            throw new CompanyCreationFailedException(
                userId,
                ex.Message,
                ex);
        }
    }

    private static InternalCreateCompanyRequest BuildCompanyPayload(
    RegisterCompanyCommand request,
    Guid userId)
    {
        return new InternalCreateCompanyRequest
        {
            OwnerUserId = userId,
            Name = request.CompanyName,
            BusinessType = request.BusinessType,
            Country = request.Country ?? string.Empty,
            City = request.City ?? string.Empty
        };
    }

    private async Task SaveIdempotencyAsync(
    Guid idempotencyKey,
    Guid userId,
    Guid companyId,
    CancellationToken ct)
    {
        await _idempotencyStore.SaveAsync(
            idempotencyKey,
            new RegisterCompanyResponseCache(
                userId,
                companyId),
            ct);
    }


    private static InternalCreateUserRequest BuildUserPayload(
    RegisterCompanyCommand request)
    {
        return new InternalCreateUserRequest
        {
            Email = request.Email,
            Password = request.Password,
            UserName = request.UserName,
            PersonName = request.PersonName,
            PhoneNumber = request.PhoneNumber
        };
    }
    private static RegisterCompanyResponse BuildResponse(
    Guid userId,
    Guid companyId)
    {
        return new()
        {
            UserId = userId,
            CompanyId = companyId,
            Status = OnboardingStatus.Completed.ToString()
        };
    }
}