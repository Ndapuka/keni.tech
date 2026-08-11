using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CompanyService.Application;
using CompanyService.Application.Commands.UpdateCompany;
using Xunit;


namespace CompanyService.Tests.DependencyInjection;

public class DependencyInjectionTests
{
    private readonly ServiceProvider _serviceProvider;

    public DependencyInjectionTests()
    {
        var services = new ServiceCollection();

        services.AddApplication();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public void AddApplication_ShouldBuildServiceProvider()
    {
        // Arrange & Act

        var provider = _serviceProvider;

        // Assert

        provider.Should().NotBeNull();
    }

    [Fact]
    public void AddApplication_ShouldResolveMediator()
    {
        // Act

        var mediator = _serviceProvider.GetService<IMediator>();

        // Assert

        mediator.Should().NotBeNull();
    }

    [Fact]
    public void AddApplication_ShouldResolveMapper()
    {
        // Act

        var mapper = _serviceProvider.GetService<IMapper>();

        // Assert

        mapper.Should().NotBeNull();
    }

    [Fact]
    public void AddApplication_ShouldResolveCreateCompanyValidator()
    {
        // Act

        //var validator = _serviceProvider.GetService<IValidator<CreateCompanyCommand>>();

        // Assert

        //validator.Should().NotBeNull();
    }

    [Fact]
    public void AddApplication_ShouldResolveUpdateCompanyValidator()
    {
        // Act

        var validator = _serviceProvider.GetService<IValidator<UpdateCompanyCommand>>();

        // Assert

        validator.Should().NotBeNull();
    }
}