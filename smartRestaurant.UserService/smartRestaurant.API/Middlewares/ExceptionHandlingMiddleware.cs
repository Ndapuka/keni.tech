using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace smartRestaurant.API.Middlewares;

public class ExceptionHandlingMiddleware
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

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception caught by middleware");

        if (context.Response.HasStarted)
        {
            _logger.LogWarning("Response has already started, cannot write error response.");
            return;
        }

        context.Response.Clear();
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();

        switch (ex)
        {
            case ValidationException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.StatusCode = context.Response.StatusCode;
                response.Message = "Validation failed";
                response.Errors = validationEx.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();
                break;

            case KeyNotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.StatusCode = context.Response.StatusCode;
                response.Message = ex.Message;
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.StatusCode = context.Response.StatusCode;
                response.Message = ex.Message;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.StatusCode = context.Response.StatusCode;

                var env = context.RequestServices.GetRequiredService<IHostEnvironment>();

                if (env.IsDevelopment())
                {
                    response.Message = ex.Message;
                    response.Errors = new List<string> { ex.InnerException?.Message ?? ex.Message };
                }
                else
                {
                    response.Message = "An unexpected error occurred";
                }
                break;
        }

        response.TraceId = context.TraceIdentifier;
        response.Timestamp = DateTime.UtcNow;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = default!;
    public List<string>? Errors { get; set; }
    public string TraceId { get; set; } = default!;
    public DateTime Timestamp { get; set; }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}

