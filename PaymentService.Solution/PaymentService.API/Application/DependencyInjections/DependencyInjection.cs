using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Behaviors;
using System.Reflection;

namespace PaymentService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);

            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
        });

        services.AddAutoMapper(assembly);

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}