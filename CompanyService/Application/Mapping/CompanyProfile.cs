using AutoMapper;
using CompanyService.Application.DTOs.Responses;
using CompanyService.Core.Entities;

namespace CompanyService.Application.Mappings;

public sealed class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<Company, CompanyResponse>()
            .ForMember(
                destination => destination.CompanyId,
                options => options.MapFrom(source => source.Id))
            .ForMember(
                destination => destination.Status,
                options => options.MapFrom(source => source.Status.ToString()))
            .ForMember(
                destination => destination.WizardStep,
                options => options.MapFrom(source => source.WizardStep.ToString()))
            .ForMember(
                destination => destination.Country,
                options => options.MapFrom(source => source.Address.Country))
            .ForMember(
                destination => destination.City,
                options => options.MapFrom(source => source.Address.City));

        CreateMap<Company, CompanyDashboardResponse>()
            .ForMember(
                destination => destination.CompanyId,
                options => options.MapFrom(source => source.Id))
            .ForMember(
                destination => destination.CompanyName,
                options => options.MapFrom(source => source.Name))
            .ForMember(
                destination => destination.Status,
                options => options.MapFrom(source => source.Status))
            .ForMember(
                destination => destination.WizardStep,
                options => options.MapFrom(source => source.WizardStep));

        CreateMap<Company, RegisterCompanyResponse>()
            .ForMember(
                destination => destination.CompanyId,
                options => options.MapFrom(source => source.Id))
            .ForMember(
                destination => destination.Status,
                options => options.MapFrom(source => source.Status.ToString()))
            .ForMember(
                destination => destination.WizardStep,
                options => options.MapFrom(source => source.WizardStep.ToString()));
    }
}