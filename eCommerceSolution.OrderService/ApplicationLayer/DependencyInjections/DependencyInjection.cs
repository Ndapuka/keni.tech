using ApplicationLayer.HttpClients;
using ApplicationLayer.HttpClientsContracts;
using ApplicationLayer.Mappings;
using ApplicationLayer.Policies;
using ApplicationLayer.ServiceContracts;
using ApplicationLayer.Services;
using ApplicationLayer.Validators;
using AutoMapper;
using ApplicationLayer.ServiceContracts;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationLayer.DependencyInjections;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(OrderMappingProfile));

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();

        // Services
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<IShippingAddressService, ShippingAddressService>();

        // Products Service
        services.AddHttpClient<IProductsServiceClient, ProductsServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:ProductsService"]!);
        })
        .AddPolicyHandler(PollyPolicies.RetryPolicy())
        .AddPolicyHandler(PollyPolicies.CircuitBreakerPolicy())
        .AddPolicyHandler(PollyPolicies.TimeoutPolicy())
        .AddPolicyHandler(PollyPolicies.FallbackPolicy());

        // Users Service
        services.AddHttpClient<IUsersServiceClient, UsersServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:UsersService"]!);
        })
        .AddPolicyHandler(PollyPolicies.RetryPolicy())
        .AddPolicyHandler(PollyPolicies.CircuitBreakerPolicy())
        .AddPolicyHandler(PollyPolicies.TimeoutPolicy())
        .AddPolicyHandler(PollyPolicies.FallbackPolicy());

        return services;
    }
}
