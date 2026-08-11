using System.Net;
using System.Text.Json;
using FluentValidation;

namespace PaymentService.API.Middlewares;

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
            _logger.LogError(exception,
                "An unhandled exception occurred.");

            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();

        switch (exception)
        {
            case ValidationException validationException:

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                response.StatusCode = context.Response.StatusCode;
                response.Message = "One or more validation errors occurred.";
                response.Errors = validationException.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                break;

            case KeyNotFoundException:

                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                response.StatusCode = context.Response.StatusCode;
                response.Message = exception.Message;

                break;

            case UnauthorizedAccessException:

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                response.StatusCode = context.Response.StatusCode;
                response.Message = exception.Message;

                break;

            default:

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                response.StatusCode = context.Response.StatusCode;
                response.Message = "An unexpected error occurred.";

                break;
        }

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }

    private sealed class ErrorResponse
    {
        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<string>? Errors { get; set; }
    }
}