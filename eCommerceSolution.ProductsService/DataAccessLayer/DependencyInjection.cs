using BusinessLogicLayer.RepositoryContracts;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using DataAccessLayer.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ProductsDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}