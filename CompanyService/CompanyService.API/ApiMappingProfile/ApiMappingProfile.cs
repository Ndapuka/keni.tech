using AutoMapper;
using CompanyService.API.Dtos.Requests.CompleteBasicInformation;
using CompanyService.API.Dtos.Requests.CompleteBranding;
using CompanyService.API.Dtos.Requests.CompleteContactInformation;
using CompanyService.API.Dtos.Requests.CompleteFiscalInformation;
using CompanyService.API.Dtos.Requests.InviteUser;
using CompanyService.API.Dtos.Requests.RegisterCompany;
using CompanyService.API.Dtos.Requests.UpdateCompany;
using CompanyService.Application.Commands.CompleteBasicInformation;
using CompanyService.Application.Commands.CompleteBranding;
using CompanyService.Application.Commands.CompleteContactInformation;
using CompanyService.Application.Commands.CompleteFiscalInformation;
using CompanyService.Application.Commands.InviteUser;
using CompanyService.Application.Commands.RegisterCompany;
using CompanyService.Application.Commands.UpdateCompany;

namespace CompanyService.API.Mappings;

public sealed class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<RegisterCompanyRequest, RegisterCompanyCommand>();

        CreateMap<UpdateCompanyRequest, UpdateCompanyCommand>();

        CreateMap<InviteUserRequest, InviteUserCommand>();

        CreateMap<
            CompleteBasicInformationRequest,
            CompleteBasicInformationCommand>();
        CreateMap<UpdateCompanyRequest, UpdateCompanyCommand>()
            .ForMember(destination => destination
            .UserId, options => options.Ignore()); // preenchido no controller a partir do JWT

        CreateMap<InviteUserRequest, InviteUserCommand>()
            .ForMember(destination => destination
            .CompanyId, options => options.Ignore()) // preenchido no controller a partir da rota
            .ForMember(
                destination => destination
                .InvitedByUserId, options => options.Ignore()); // preenchido no controller a partir do JWT


        CreateMap<
            CompleteContactInformationRequest,
            CompleteContactInformationCommand>();

        CreateMap<
            CompleteFiscalInformationRequest,
            CompleteFiscalInformationCommand>();

        CreateMap<
            CompleteBrandingRequest,
            CompleteBrandingCommand>();
    }
}