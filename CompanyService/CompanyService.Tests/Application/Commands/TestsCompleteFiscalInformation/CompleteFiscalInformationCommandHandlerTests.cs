using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Application.Commands.CompleteFiscalInformation;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Entities;
using CompanyService.Core.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CompanyService.Tests.Application.Commands.TestsCompleteFiscalInformation;

public sealed class CompleteFiscalInformationCommandHandlerTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CompleteFiscalInformationCommandHandler>> _loggerMock;

    private readonly CompleteFiscalInformationCommandHandler _handler;

    public CompleteFiscalInformationCommandHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock =
            new Mock<ILogger<CompleteFiscalInformationCommandHandler>>();

        _handler = new CompleteFiscalInformationCommandHandler(
            _companyRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCompleteFiscalInformationSuccessfully()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var command = new CompleteFiscalInformationCommand
        {
            CompanyId = companyId,
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
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
        company.TaxNumber
            .Should()
            .Be("PT123456789");

        company.Address
            .Should()
            .NotBeNull();

        company.Address.Street
            .Should()
            .Be("Rua Principal");

        company.Address.City
            .Should()
            .Be("Coimbra");

        company.Address.PostalCode
            .Should()
            .Be("3000-000");

        company.Address.Country
            .Should()
            .Be("Portugal");

        company.WizardStep
            .Should()
            .Be(CompanyWizardStep.Branding);

        company.Status
            .Should()
            .Be(CompanyStatus.PendingConfiguration);

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

        var command = new CompleteFiscalInformationCommand
        {
            CompanyId = companyId,
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
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
    public async Task Handle_ShouldCreateAddressFromCommandValues()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var command = new CompleteFiscalInformationCommand
        {
            CompanyId = companyId,
            TaxNumber = "AO987654321",
            Street = "Rua da Independência",
            City = "Lubango",
            PostalCode = "1000-100",
            Country = "Angola"
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
        company.TaxNumber
            .Should()
            .Be(command.TaxNumber);

        company.Address.Street
            .Should()
            .Be(command.Street);

        company.Address.City
            .Should()
            .Be(command.City);

        company.Address.PostalCode
            .Should()
            .Be(command.PostalCode);

        company.Address.Country
            .Should()
            .Be(command.Country);
    }

    [Fact]
    public async Task Handle_ShouldMoveWizardToBranding()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var command = new CompleteFiscalInformationCommand
        {
            CompanyId = companyId,
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
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
        company.WizardStep
            .Should()
            .Be(CompanyWizardStep.Branding);

        company.Status
            .Should()
            .Be(CompanyStatus.PendingConfiguration);

        _companyRepositoryMock.Verify(
            x => x.Update(
                It.Is<Company>(x =>
                    x == company &&
                    x.TaxNumber == command.TaxNumber &&
                    x.WizardStep == CompanyWizardStep.Branding)),
            Times.Once);
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

        var command = new CompleteFiscalInformationCommand
        {
            CompanyId = companyId,
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
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
                    x.TaxNumber == command.TaxNumber &&
                    x.Address.Street == command.Street &&
                    x.Address.City == command.City &&
                    x.Address.PostalCode == command.PostalCode &&
                    x.Address.Country == command.Country &&
                    x.WizardStep == CompanyWizardStep.Branding)),
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

        using var cancellationTokenSource = new CancellationTokenSource();

        var cancellationToken = cancellationTokenSource.Token;

        var command = new CompleteFiscalInformationCommand
        {
            CompanyId = companyId,
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
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
}