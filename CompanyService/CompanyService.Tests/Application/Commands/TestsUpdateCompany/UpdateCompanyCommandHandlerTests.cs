using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Application.Commands.UpdateCompany;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Entities;
using CompanyService.Core.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CompanyService.Tests.Application.Commands.TestsUpdateCompany;

public sealed class UpdateCompanyCommandHandlerTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<UpdateCompanyCommandHandler>> _loggerMock;

    private readonly UpdateCompanyCommandHandler _handler;

    public UpdateCompanyCommandHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock =
            new Mock<ILogger<UpdateCompanyCommandHandler>>();

        _handler = new UpdateCompanyCommandHandler(
            _companyRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateCompanySuccessfully()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var command = new UpdateCompanyCommand
        {
            CompanyId = companyId,
            Name = "Keni New",
            BusinessType = BusinessType.Bakery
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(
                companyId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        company.Name
            .Should()
            .Be("Keni New");

        company.BusinessType
            .Should()
            .Be(BusinessType.Bakery);

        _companyRepositoryMock.Verify(
            x => x.GetByIdAsync(
                companyId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _companyRepositoryMock.Verify(
            x => x.Update(company),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCompanyDoesNotExist_ShouldThrowCompanyNotFoundException()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var command = new UpdateCompanyCommand
        {
            CompanyId = companyId,
            Name = "Keni New",
            BusinessType = BusinessType.Restaurant
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(
                companyId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company?)null);

        // Act
        var act = () =>
            _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<CompanyNotFoundException>();

        _companyRepositoryMock.Verify(
            x => x.GetByIdAsync(
                companyId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _companyRepositoryMock.Verify(
            x => x.Update(It.IsAny<Company>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldUpdateOnlyNameAndBusinessType()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var ownerUserId = Guid.NewGuid();

        var company = Company.Register(
            ownerUserId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        company.CompleteBasicInformation("keni");

        var originalId = company.Id;
        var originalOwnerId = company.OwnerUserId;
        var originalSlug = company.Slug;
        var originalCountry = company.Address.Country;
        var originalCity = company.Address.City;
        var originalStatus = company.Status;
        var originalWizardStep = company.WizardStep;

        var command = new UpdateCompanyCommand
        {
            CompanyId = companyId,
            Name = "Keni Updated",
            BusinessType = BusinessType.Barbershop
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(
                companyId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        company.Id
            .Should()
            .Be(originalId);

        company.OwnerUserId
            .Should()
            .Be(originalOwnerId);

        company.Name
            .Should()
            .Be("Keni Updated");

        company.BusinessType
            .Should()
            .Be(BusinessType.Barbershop);

        company.Slug
            .Should()
            .Be(originalSlug);

        company.Address.Country
            .Should()
            .Be(originalCountry);

        company.Address.City
            .Should()
            .Be(originalCity);

        company.Status
            .Should()
            .Be(originalStatus);

        company.WizardStep
            .Should()
            .Be(originalWizardStep);
    }

    [Fact]
    public async Task Handle_ShouldPersistUpdatedCompany()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var command = new UpdateCompanyCommand
        {
            CompanyId = companyId,
            Name = "Keni Updated",
            BusinessType = BusinessType.CoffeeShop
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(
                companyId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        _companyRepositoryMock.Verify(
            x => x.Update(
                It.Is<Company>(x =>
                    x == company &&
                    x.Name == command.Name &&
                    x.BusinessType == command.BusinessType)),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        var command = new UpdateCompanyCommand
        {
            CompanyId = companyId,
            Name = "Keni Updated",
            BusinessType = BusinessType.Barbershop
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(
                companyId,
                cancellationToken))
            .ReturnsAsync(company);

        // Act
        await _handler.Handle(
            command,
            cancellationToken);

        // Assert
        _companyRepositoryMock.Verify(
            x => x.GetByIdAsync(
                companyId,
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToUnitOfWork()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        var command = new UpdateCompanyCommand
        {
            CompanyId = companyId,
            Name = "Keni Updated",
            BusinessType = BusinessType.Restaurant
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(
                companyId,
                cancellationToken))
            .ReturnsAsync(company);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(
                cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(
            command,
            cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                cancellationToken),
            Times.Once);
    }
}
