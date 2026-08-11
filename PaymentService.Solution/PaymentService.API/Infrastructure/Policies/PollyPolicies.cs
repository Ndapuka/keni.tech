using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System.Net;

namespace PaymentService.Infrastructure.Policies;

public static class PollyPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> RetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(response => response.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30));
    }

    public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy()
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(
            TimeSpan.FromSeconds(30),
            TimeoutStrategy.Optimistic);
    }
}