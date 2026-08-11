using Microsoft.Extensions.DependencyInjection;
namespace smartRestaurant.Core;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method to add Core services to the dependency injection container.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        ////TO DO: Add service to the IoC container
        ////Core services often include data access, caching and other low-level components.


        return services;
    }
}

