using ApplicationLayer.Mappings;
using ApplicationLayer.ServiceContracts;
using ApplicationLayer.Services;
using ApplicationLayer.Validators.Categories;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace ApplicationLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(CategoryMappingProfile).Assembly); //erro cs1503 argument cannot convert for system.reflection.assembly..

        services.AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>();// cs1061 do not contain definition to 
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddScoped<IProductService, ProductService>();

        services.AddScoped<IProductImageService, ProductImageService>();

        return services;
    }
}
