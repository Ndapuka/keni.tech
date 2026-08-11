using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onboarding.Application.Idempotency;
using Onboarding.Application.Interfaces;
using Onboarding.Infrastructure.Clients;
using Onboarding.Infrastructure.Configuration;
using Onboarding.Infrastructure.Idempotency;
using Polly;
using Polly.Extensions.Http;

namespace Onboarding.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOnboardingInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServiceEndpointsOptions>(configuration.GetSection(ServiceEndpointsOptions.SectionName));
        var endpoints = configuration.GetSection(ServiceEndpointsOptions.SectionName).Get<ServiceEndpointsOptions>()
            ?? throw new InvalidOperationException("ServiceEndpoints configuration section is missing.");

        services.AddHttpClient<IUserClient, UserClient>(client =>
        {
            client.BaseAddress = new Uri(endpoints.UserServiceBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(10);
        })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<ICompanyClient, CompanyClient>(client =>
        {
            client.BaseAddress = new Uri(endpoints.CompanyServiceBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(10);
        })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddSingleton<IIdempotencyStore, InMemoryIdempotencyStore>();

        return services;
    }

    // 3 tentativas, backoff exponencial — cobre falhas transitórias de rede sem mascarar falhas reais
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)));

    // Depois de 5 falhas seguidas, abre o circuito por 30s — evita martelar um serviço já caído
    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
