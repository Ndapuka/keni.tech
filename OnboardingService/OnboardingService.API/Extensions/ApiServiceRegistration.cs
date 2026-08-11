using Microsoft.OpenApi.Models;
using Onboarding.API.HealthChecks;
using Onboarding.API.Middleware;

namespace Onboarding.API.Extensions;

public static class ApiServiceRegistration
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerDocumentation();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddProblemDetails();

        services.AddHttpClient("HealthChecks", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(2);
        });

        services.AddHealthChecks()
            .AddCheck<DependentServicesHealthCheck>("dependent_services");

        return services;
    }
}