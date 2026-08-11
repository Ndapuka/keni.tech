using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Application.Commands.RegisterCompany;
using CompanyService.Application.DTOs.Responses;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CompanyService.Tests.Application.Commands.TestsRegisterCompany;

public sealed class RegisterCompanyCommandHandlerTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<RegisterCompanyCommandHandler>> _loggerMock;

    private readonly RegisterCompanyCommandHandler _handler;

    public RegisterCompanyCommandHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock =
            new Mock<ILogger<RegisterCompanyCommandHandler>>();

        _handler = new RegisterCompanyCommandHandler(
            _companyRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRegisterCompanySuccessfully()
    {
        // Arrange
        var ownerUserId = Guid.NewGuid();

        var command = new RegisterCompanyCommand
        {
            OwnerUserId = ownerUserId,
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        Company? addedCompany = null;

        _companyRepositoryMock
            .Setup(x => x.AddAsync(
                It.IsAny<Company>(),
                It.IsAny<CancellationToken>()))
            .Callback<Company, CancellationToken>(
                (company, _) => addedCompany = company)
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var response = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        response.Should().NotBeNull();

        response.CompanyId
            .Should()
            .NotBe(Guid.Empty);

        response.Status
            .Should()
            .Be(CompanyStatus.PendingConfiguration.ToString());

        response.WizardStep
            .Should()
            .Be(CompanyWizardStep.BasicInformation.ToString());

        addedCompany.Should().NotBeNull();

        addedCompany!.Id
            .Should()
            .Be(response.CompanyId);

        addedCompany.OwnerUserId
            .Should()
            .Be(ownerUserId);

        addedCompany.Name
            .Should()
            .Be("Keni");

        addedCompany.BusinessType
            .Should()
            .Be(BusinessType.Restaurant);

        addedCompany.Address.Country
            .Should()
            .Be("Portugal");

        addedCompany.Address.City
            .Should()
            .Be("Coimbra");

        addedCompany.Status
            .Should()
            .Be(CompanyStatus.PendingConfiguration);

        addedCompany.WizardStep
            .Should()
            .Be(CompanyWizardStep.BasicInformation);
    }

    [Fact]
    public async Task Handle_ShouldAddCompanyToRepository()
    {
        // Arrange
        var command = new RegisterCompanyCommand
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        _companyRepositoryMock
            .Setup(x => x.AddAsync(
                It.IsAny<Company>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        _companyRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<Company>(company =>
                    company.OwnerUserId == command.OwnerUserId &&
                    company.Name == command.Name &&
                    company.BusinessType == command.BusinessType &&
                    company.Address.Country == command.Country &&
                    company.Address.City == command.City),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldSaveChanges()
    {
        // Arrange
        var command = new RegisterCompanyCommand
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        _companyRepositoryMock
            .Setup(x => x.AddAsync(
                It.IsAny<Company>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectCompanyId()
    {
        // Arrange
        var command = new RegisterCompanyCommand
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        Company? addedCompany = null;

        _companyRepositoryMock
            .Setup(x => x.AddAsync(
                It.IsAny<Company>(),
                It.IsAny<CancellationToken>()))
            .Callback<Company, CancellationToken>(
                (company, _) => addedCompany = company)
            .Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        addedCompany.Should().NotBeNull();

        response.CompanyId
            .Should()
            .Be(addedCompany!.Id);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var command = new RegisterCompanyCommand
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        _companyRepositoryMock
            .Setup(x => x.AddAsync(
                It.IsAny<Company>(),
                cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(
            command,
            cancellationToken);

        // Assert
        _companyRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Company>(),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToUnitOfWork()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var command = new RegisterCompanyCommand
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        _companyRepositoryMock
            .Setup(x => x.AddAsync(
                It.IsAny<Company>(),
                cancellationToken))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(
            command,
            cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(cancellationToken),
            Times.Once);
    }
}
