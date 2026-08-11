using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using smartRestaurant.Application.ServiceContracts;
using smartRestaurant.Application.Services;
using smartRestaurant.Application.Validators;
using smartRestaurant.Core.Entities;
using smartRestaurant.Core.RepositoryContracts;
using System.Reflection;
using EmailService = smartRestaurant.Application.Services.EmailService;

namespace smartRestaurant.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method to add Core services to the dependency injection container.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ////TO DO: Add service to the IoC container
        ////Application services often include business logic, validation, and other high-level components.
        services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());





        return services;
    }
}

