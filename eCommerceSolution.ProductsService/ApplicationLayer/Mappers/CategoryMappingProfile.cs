using ApplicationLayer.DTOs.Categories;
using AutoMapper;
using BusinessLogicLayer.Entities;

namespace ApplicationLayer.Mappings;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryResponseDto>();

        CreateMap<CreateCategoryRequestDto, Category>();

        CreateMap<UpdateCategoryRequestDto, Category>();
    }
}