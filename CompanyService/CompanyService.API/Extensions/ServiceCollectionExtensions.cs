using CompanyService.API.Mappings;
using CompanyService.Application;
using CompanyService.Infrastructure;
namespace CompanyService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();

        services.AddAutoMapper(
            typeof(ApiMappingProfile).Assembly);

        services.AddEndpointsApiExplorer();

        services.AddApplication();

        services.AddInfrastructure(configuration);

        services.AddJwtAuthentication(configuration);

        services.AddSwaggerDocumentation();

        return services;
    }
}