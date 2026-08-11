using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Onboarding.Infrastructure.Configuration;

namespace Onboarding.API.HealthChecks;

public sealed class DependentServicesHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ServiceEndpointsOptions _endpoints;

    public DependentServicesHealthCheck(
        IHttpClientFactory httpClientFactory,
        IOptions<ServiceEndpointsOptions> endpoints)
    {
        _httpClientFactory = httpClientFactory;
        _endpoints = endpoints.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        // HttpClient configurado no DependencyInjection
        var client = _httpClientFactory.CreateClient("HealthChecks");

        // Executa todos os health checks em paralelo
        var tasks = new[]
        {
            CheckServiceAsync(client, "UserService", _endpoints.UserServiceBaseUrl, cancellationToken),
            CheckServiceAsync(client, "CompanyService", _endpoints.CompanyServiceBaseUrl, cancellationToken)
        };

        var results = await Task.WhenAll(tasks);

        var failures = results
            .Where(result => result is not null)
            .Cast<string>()
            .ToList();

        return failures.Count switch
        {
            0 => HealthCheckResult.Healthy(
                "All dependent services are reachable."),

            1 => HealthCheckResult.Degraded(
                $"Unreachable service: {failures.First()}."),

            _ => HealthCheckResult.Unhealthy(
                $"Multiple dependent services are unavailable: {string.Join(", ", failures)}.")
        };
    }

    private static async Task<string?> CheckServiceAsync(
        HttpClient client,
        string serviceName,
        string baseUrl,
        CancellationToken cancellationToken)
    {
        try
        {
            // Garante que o endpoint fica corretamente formado
            var endpoint = new Uri(new Uri(baseUrl), "health");

            var response = await client.GetAsync(endpoint, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return $"{serviceName} ({(int)response.StatusCode} - {response.ReasonPhrase})";
            }

            return null;
        }
        catch (Exception ex)
        {
            return $"{serviceName} ({ex.Message})";
        }
    }
}