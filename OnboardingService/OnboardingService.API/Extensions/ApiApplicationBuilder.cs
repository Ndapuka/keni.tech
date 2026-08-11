using Scalar.AspNetCore;

namespace Onboarding.API.Extensions;

public static class ApiApplicationBuilder
{
    public static WebApplication UseApiPipeline(
        this WebApplication app)
    {
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
                    "Onboarding Service API v1");

                options.RoutePrefix = "swagger";
            });

            app.MapScalarApiReference(options =>
            {
                options.Title = "Onboarding Service API";
                options.Theme = ScalarTheme.Default;
            });
        }

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.MapHealthChecks("/health");

        return app;
    }
}
