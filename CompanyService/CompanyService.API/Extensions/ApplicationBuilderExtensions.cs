using CompanyService.API.Middleware;
using Scalar.AspNetCore;

namespace CompanyService.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseApplicationPipeline(
    this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "openapi/{documentName}.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    "/openapi/v1.json",
                    "Company Service API v1");

                options.RoutePrefix = "swagger";
            });

            app.MapScalarApiReference(options =>
            {
                options.Title = "Company Service API";
                options.Theme = ScalarTheme.Default;
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
