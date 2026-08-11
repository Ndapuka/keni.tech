using CompanyService.Core.Exceptions;
using System.Text.Json;

namespace CompanyService.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "An unhandled exception occurred while processing request {Method} {Path}.",
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(
                context,
                exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,

            CompanyNotFoundException => StatusCodes.Status404NotFound,
            KeyNotFoundException => StatusCodes.Status404NotFound,

            CompanyMembershipRequiredException => StatusCodes.Status403Forbidden,
            InsufficientCompanyRoleException => StatusCodes.Status403Forbidden,

            InvalidOperationException => StatusCodes.Status409Conflict,

            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var response = new ProblemDetailsResponse
        {
            Title = GetTitle(statusCode),
            Status = statusCode,
            Detail = exception.Message,
            Instance = context.Request.Path,
            TraceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            StatusCodes.Status403Forbidden => "Forbidden",
            StatusCodes.Status404NotFound => "Resource Not Found",
            StatusCodes.Status409Conflict => "Business Rule Violation",
            _ => "Internal Server Error"
        };
    }

    private sealed class ProblemDetailsResponse
    {
        public string Title { get; init; } = string.Empty;

        public int Status { get; init; }

        public string Detail { get; init; } = string.Empty;

        public string Instance { get; init; } = string.Empty;

        public string TraceId { get; init; } = string.Empty;
    }
}