using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace ApplicationLayer.Policies;

public static class PollyPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> RetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
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
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30));
    }

    public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy()
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(10);
    }

    public static IAsyncPolicy<HttpResponseMessage> FallbackPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<Exception>()
            .FallbackAsync(new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable));
    }
}