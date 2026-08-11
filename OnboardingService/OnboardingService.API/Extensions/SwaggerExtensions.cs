
using Microsoft.OpenApi.Models;
using Onboarding.API.HealthChecks;
using Onboarding.API.Middleware;

namespace Onboarding.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(
        this IServiceCollection services)
    {


        services.AddEndpointsApiExplorer();


        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "KeNI Onboarding API",
                Version = "v1",
                Description = "Onboarding microservice responsible for company and owner registration."
            });
        });

        services.AddExceptionHandler<GlobalExceptionHandler>();


        return services;
    }
}