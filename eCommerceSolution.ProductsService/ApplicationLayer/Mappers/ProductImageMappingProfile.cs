using ApplicationLayer.DTOs.ProductImages;
using AutoMapper;
using BusinessLogicLayer.Entities;

namespace ApplicationLayer.Mappings;

public class ProductImageMappingProfile : Profile
{
    public ProductImageMappingProfile()
    {
        CreateMap<ProductImage, ProductImageResponseDto>();

        CreateMap<CreateProductImageRequestDto, ProductImage>();
    }
}
