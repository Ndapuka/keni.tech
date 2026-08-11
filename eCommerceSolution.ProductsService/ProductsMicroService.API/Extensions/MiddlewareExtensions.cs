using ProductsMicroservice.API.Middlewares;

namespace ProductsMicroservice.API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
