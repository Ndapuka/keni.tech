using AutoMapper;
using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.API.Dtos.Requests.CompleteBasicInformation;
using CompanyService.API.Dtos.Requests.CompleteBranding;
using CompanyService.API.Dtos.Requests.CompleteContactInformation;
using CompanyService.API.Dtos.Requests.CompleteFiscalInformation;
using CompanyService.API.Dtos.Requests.InviteUser;
using CompanyService.API.Dtos.Requests.RegisterCompany;
using CompanyService.API.Dtos.Requests.UpdateCompany;
using CompanyService.API.Mappings;
using CompanyService.Application.Commands.CompleteBasicInformation;
using CompanyService.Application.Commands.CompleteBranding;
using CompanyService.Application.Commands.CompleteContactInformation;
using CompanyService.Application.Commands.CompleteFiscalInformation;
using CompanyService.Application.Commands.InviteUser;
using CompanyService.Application.Commands.RegisterCompany;
using CompanyService.Application.Commands.UpdateCompany;
using FluentAssertions;
using Xunit;

namespace CompanyService.Tests.API.Mappings;

public sealed class ApiMappingProfileTests
{
    private readonly IMapper _mapper;

    public ApiMappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ApiMappingProfile>();
        });

        configuration.AssertConfigurationIsValid();

        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void Should_Have_Valid_Api_Mapping_Configuration()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ApiMappingProfile>();
        });

        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Should_Map_RegisterCompanyRequest_To_RegisterCompanyCommand()
    {
        var request = new RegisterCompanyRequest
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        var command = _mapper.Map<RegisterCompanyCommand>(request);

        command.OwnerUserId.Should().Be(request.OwnerUserId);
        command.Name.Should().Be(request.Name);
        command.BusinessType.Should().Be(request.BusinessType);
        command.Country.Should().Be(request.Country);
        command.City.Should().Be(request.City);
    }

    [Fact]
    public void Should_Map_UpdateCompanyRequest_To_UpdateCompanyCommand()
    {
        var request = new UpdateCompanyRequest
        {
            CompanyId = Guid.NewGuid(),
            Name = "Keni Updated",
            BusinessType = BusinessType.Restaurant
        };

        var command = _mapper.Map<UpdateCompanyCommand>(request);

        command.CompanyId.Should().Be(request.CompanyId);
        command.Name.Should().Be(request.Name);
        command.BusinessType.Should().Be(request.BusinessType);
    }

    [Fact]
    public void Should_Map_InviteUserRequest_To_InviteUserCommand()
    {
        var request = new InviteUserRequest
        {

            UserId = Guid.NewGuid(),
            Role = CompanyRole.Manager
        };

        var command = _mapper.Map<InviteUserCommand>(request);


        command.UserId.Should().Be(request.UserId);
        command.Role.Should().Be(request.Role);
    }

    [Fact]
    public void Should_Map_CompleteBasicInformationRequest_To_Command()
    {
        var request = new CompleteBasicInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Slug = "keni-restaurant"
        };

        var command =
            _mapper.Map<CompleteBasicInformationCommand>(request);

        command.CompanyId.Should().Be(request.CompanyId);
        command.Slug.Should().Be(request.Slug);
    }

    [Fact]
    public void Should_Map_CompleteContactInformationRequest_To_Command()
    {
        var request = new CompleteContactInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Email = "contact@keni.com",
            Phone = "+351912345678"
        };

        var command =
            _mapper.Map<CompleteContactInformationCommand>(request);

        command.CompanyId.Should().Be(request.CompanyId);
        command.Email.Should().Be(request.Email);
        command.Phone.Should().Be(request.Phone);
    }

    [Fact]
    public void Should_Map_CompleteFiscalInformationRequest_To_Command()
    {
        var request = new CompleteFiscalInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
        };

        var command =
            _mapper.Map<CompleteFiscalInformationCommand>(request);

        command.CompanyId.Should().Be(request.CompanyId);
        command.TaxNumber.Should().Be(request.TaxNumber);
        command.Street.Should().Be(request.Street);
        command.City.Should().Be(request.City);
        command.PostalCode.Should().Be(request.PostalCode);
        command.Country.Should().Be(request.Country);
    }

    [Fact]
    public void Should_Map_CompleteBrandingRequest_To_Command()
    {
        var request = new CompleteBrandingRequest
        {
            CompanyId = Guid.NewGuid(),
            Description = "Restaurant Keni",
            LogoUrl = "https://keni.com/logo.png"
        };

        var command =
            _mapper.Map<CompleteBrandingCommand>(request);

        command.CompanyId.Should().Be(request.CompanyId);
        command.Description.Should().Be(request.Description);
        command.LogoUrl.Should().Be(request.LogoUrl);
    }
}
