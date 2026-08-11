using PaymentService.Application;
using PaymentService.Infrastructure;

namespace PaymentService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        return services;
    }
}