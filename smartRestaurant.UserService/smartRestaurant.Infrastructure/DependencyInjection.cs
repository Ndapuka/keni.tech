using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using smartRestaurant.Application.ServiceContracts;
using smartRestaurant.Core.RepositoryContracts;
using smartRestaurant.Core.UnitOfWorkContrats;
using smartRestaurant.Infrastructure.Clients;
using smartRestaurant.Infrastructure.Persistence;
using smartRestaurant.Infrastructure.Repositories;
using smartRestaurant.Infrastructure.UnitOfWorkImplement;

namespace smartRestaurant.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<ICompanyServiceClient, CompanyServiceClient>(
            client =>
            {
                client.BaseAddress = new Uri(
                    configuration["ServiceEndpoints:CompanyService"]!
                );
            });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddTransient<IUsersRepository, UsersRepository>();

        services.AddScoped<IUserTokenRepository, UserTokenRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}


