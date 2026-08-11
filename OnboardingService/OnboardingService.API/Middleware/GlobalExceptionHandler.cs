using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Onboarding.Domain.Exceptions;

namespace Onboarding.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, stage, logAsCritical) = MapException(exception);

        if (logAsCritical)
        {
            _logger.LogCritical(exception,
                "Unhandled critical exception. Stage: {Stage}. TraceId: {TraceId}. Path: {Path}",
                stage, httpContext.TraceIdentifier, httpContext.Request.Path);
        }
        else
        {
            _logger.LogWarning(exception,
                "Handled onboarding exception. Stage: {Stage}. TraceId: {TraceId}. Path: {Path}",
                stage, httpContext.TraceIdentifier, httpContext.Request.Path);
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = stage,
            Detail = exception.Message,
            Instance = httpContext.Request.Path,
            Extensions = { ["traceId"] = httpContext.TraceIdentifier }
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; // exceção tratada — não propaga para o pipeline default do ASP.NET
    }

    // Fonte única de verdade: que exceção de domínio mapeia para que resposta HTTP.
    // Adicionar uma exceção nova ao Domain só exige uma linha aqui, nunca tocar em controllers.
    private static (int StatusCode, string Stage, bool IsCritical) MapException(Exception exception) => exception switch
    {
        CompensationFailedException => (StatusCodes.Status500InternalServerError, "Compensation", true),
        CompanyCreationFailedException => (StatusCodes.Status422UnprocessableEntity, "CompanyCreation", false),
        UserCreationFailedException => (StatusCodes.Status422UnprocessableEntity, "UserCreation", false),
        OnboardingDomainException => (StatusCodes.Status422UnprocessableEntity, "Onboarding", false),
        _ => (StatusCodes.Status500InternalServerError, "Unexpected", true)
    };
}
