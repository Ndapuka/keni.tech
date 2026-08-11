using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Infrastructure.Persistence.Context;
using CompanyService.Infrastructure.Persistence.Repositories;
using CompanyService.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        RegisterDatabase(services, configuration);

        RegisterRepositories(services);

        return services;
    }

    private static void RegisterDatabase(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CompanyDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });
    }

    private static void RegisterRepositories(
        IServiceCollection services)
    {
        services.AddScoped<ICompanyRepository, CompanyRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}