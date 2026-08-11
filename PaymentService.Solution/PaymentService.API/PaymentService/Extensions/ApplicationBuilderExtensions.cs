using PaymentService.API.Middlewares;
using Scalar.AspNetCore;

namespace PaymentService.API.Extensions;

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

                options.RouteTemplate = "/openapi/{documentName}.json";
            });
            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/openapi/v1.json", "Users API v1");
                c.RoutePrefix = "swagger";
            });
            app.MapScalarApiReference(options =>
            {
                options.Title = "Payment Microservice API";
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
