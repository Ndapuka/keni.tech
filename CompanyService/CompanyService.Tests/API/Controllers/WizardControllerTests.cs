using AutoMapper;
using CompanyService.API.Controllers;
using CompanyService.API.Dtos.Requests.CompleteBasicInformation;
using CompanyService.API.Dtos.Requests.CompleteBranding;
using CompanyService.API.Dtos.Requests.CompleteContactInformation;
using CompanyService.API.Dtos.Requests.CompleteFiscalInformation;
using CompanyService.Application.Commands.CompleteBasicInformation;
using CompanyService.Application.Commands.CompleteBranding;
using CompanyService.Application.Commands.CompleteContactInformation;
using CompanyService.Application.Commands.CompleteFiscalInformation;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CompanyService.Tests.API.Controllers;

public sealed class WizardControllerTests
{
    private readonly Mock<ISender> _senderMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly WizardController _controller;

    public WizardControllerTests()
    {
        _senderMock = new Mock<ISender>();
        _mapperMock = new Mock<IMapper>();

        _controller = new WizardController(
            _senderMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task CompleteBasicInformation_Should_Return_NoContent()
    {
        // Arrange
        var request = new CompleteBasicInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Slug = "keni-restaurant"
        };

        var command = new CompleteBasicInformationCommand
        {
            CompanyId = request.CompanyId,
            Slug = request.Slug
        };

        _mapperMock
            .Setup(x => x.Map<CompleteBasicInformationCommand>(request))
            .Returns(command);

        _senderMock
            .Setup(x => x.Send(
                command,
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.CompleteBasicInformation(
            request.CompanyId,
            request,
            CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mapperMock.Verify(
            x => x.Map<CompleteBasicInformationCommand>(request),
            Times.Once);

        _senderMock.Verify(
            x => x.Send(
                command,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CompleteContactInformation_Should_Return_NoContent()
    {
        // Arrange
        var request = new CompleteContactInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Email = "contact@keni.com",
            Phone = "+351912345678"
        };

        var command = new CompleteContactInformationCommand
        {
            CompanyId = request.CompanyId,
            Email = request.Email,
            Phone = request.Phone
        };

        _mapperMock
            .Setup(x => x.Map<CompleteContactInformationCommand>(request))
            .Returns(command);

        _senderMock
            .Setup(x => x.Send(
                command,
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.CompleteContactInformation(
            request.CompanyId,
            request,
            CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mapperMock.Verify(
            x => x.Map<CompleteContactInformationCommand>(request),
            Times.Once);

        _senderMock.Verify(
            x => x.Send(
                command,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CompleteFiscalInformation_Should_Return_NoContent()
    {
        // Arrange
        var request = new CompleteFiscalInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
        };

        var command = new CompleteFiscalInformationCommand
        {
            CompanyId = request.CompanyId,
            TaxNumber = request.TaxNumber,
            Street = request.Street,
            City = request.City,
            PostalCode = request.PostalCode,
            Country = request.Country
        };

        _mapperMock
            .Setup(x => x.Map<CompleteFiscalInformationCommand>(request))
            .Returns(command);

        _senderMock
            .Setup(x => x.Send(
                command,
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.CompleteFiscalInformation(
            request.CompanyId,
            request,
            CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mapperMock.Verify(
            x => x.Map<CompleteFiscalInformationCommand>(request),
            Times.Once);

        _senderMock.Verify(
            x => x.Send(
                command,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CompleteBranding_Should_Return_NoContent()
    {
        // Arrange
        var request = new CompleteBrandingRequest
        {
            CompanyId = Guid.NewGuid(),
            Description = "Restaurant Keni",
            LogoUrl = "https://keni.com/logo.png"
        };

        var command = new CompleteBrandingCommand
        {
            CompanyId = request.CompanyId,
            Description = request.Description,
            LogoUrl = request.LogoUrl
        };

        _mapperMock
            .Setup(x => x.Map<CompleteBrandingCommand>(request))
            .Returns(command);

        _senderMock
            .Setup(x => x.Send(
                command,
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.CompleteBranding(
            request.CompanyId,
            request,
            CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mapperMock.Verify(
            x => x.Map<CompleteBrandingCommand>(request),
            Times.Once);

        _senderMock.Verify(
            x => x.Send(
                command,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
