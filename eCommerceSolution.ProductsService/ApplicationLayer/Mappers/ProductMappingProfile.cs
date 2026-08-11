using ApplicationLayer.DTOs.Products;
using AutoMapper;
using BusinessLogicLayer.Entities;

namespace ApplicationLayer.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CreateProductRequestDto, Product>();

        CreateMap<UpdateProductRequestDto, Product>();
    }
}