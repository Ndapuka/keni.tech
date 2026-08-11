using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.ServiceContracts;
using PaymentService.Core.Interfaces.Common;
using PaymentService.Core.Interfaces.Repositories;
using PaymentService.Infrastructure.Gateways.MbWay;
using PaymentService.Infrastructure.Gateways.Visa;
using PaymentService.Infrastructure.Persistence;
using PaymentService.Infrastructure.Persistence.Repositories;
using PaymentService.Infrastructure.Policies;
using PaymentService.Infrastructure.Services;


namespace PaymentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<PaymentDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        // Options
        services.Configure<MbWayOptions>(
            configuration.GetSection(MbWayOptions.SectionName));

        services.Configure<VisaOptions>(
            configuration.GetSection(VisaOptions.SectionName));

        // HttpContext
        services.AddHttpContextAccessor();

        // Repositories
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentAttemptRepository, PaymentAttemptRepository>();
        services.AddScoped<IPaymentAuditRepository, PaymentAuditRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();

        // Gateways (Strategy Pattern)
        services.AddHttpClient<IPaymentGateway, MbWayGateway>()
            .AddPolicyHandler(PollyPolicies.RetryPolicy())
            .AddPolicyHandler(PollyPolicies.CircuitBreakerPolicy())
            .AddPolicyHandler(PollyPolicies.TimeoutPolicy());

        services.AddHttpClient<IPaymentGateway, VisaGateway>()
            .AddPolicyHandler(PollyPolicies.RetryPolicy())
            .AddPolicyHandler(PollyPolicies.CircuitBreakerPolicy())
            .AddPolicyHandler(PollyPolicies.TimeoutPolicy());

        return services;
    }
}