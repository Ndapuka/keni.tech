using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Onboarding.Application.Compensation;
using Onboarding.Application.Interfaces;

namespace Onboarding.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOnboardingApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<ICompensationService, CompensationService>();

        return services;
    }
}
