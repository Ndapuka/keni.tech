
using AutoMapper;
using smartRestaurant.Application.DTO;
using smartRestaurant.Core.Entities;

namespace smartRestaurant.API.Mappers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // ApplicationUser → UserDto
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserID))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed));

        // ApplicationUser → AuthenticationResponse
        CreateMap<ApplicationUser, AuthenticationResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserID));

        // UpdateUserRequest → ApplicationUser (para updates)
        CreateMap<UpdateUserRequest, ApplicationUser>()
            .ForMember(dest => dest.UserID, opt => opt.Ignore())          // nunca atualizar ID
            .ForMember(dest => dest.Email, opt => opt.Ignore())           // email não muda
            .ForMember(dest => dest.Role, opt => opt.Ignore())            // role não muda
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())    // password não muda aqui
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());      // não mexer na data de criação
    }
}

