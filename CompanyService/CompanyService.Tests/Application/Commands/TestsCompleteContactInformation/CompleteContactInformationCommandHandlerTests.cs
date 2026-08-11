using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Application.Commands.CompleteContactInformation;

using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Entities;
using CompanyService.Core.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CompanyService.Tests.Application.Commands.TestsCompleteContactInformation;

public sealed class CompleteContactInformationCommandHandlerTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CompleteContactInformationCommandHandler>> _loggerMock;

    private readonly CompleteContactInformationCommandHandler _handler;

    public CompleteContactInformationCommandHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock =
            new Mock<ILogger<CompleteContactInformationCommandHandler>>();

        _handler = new CompleteContactInformationCommandHandler(
            _companyRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCompleteContactInformationSuccessfully()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var command = new CompleteContactInformationCommand
        {
            CompanyId = companyId,
            Email = "contact@keni.com",
            Phone = "+351912345678"
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
        company.Contact.Should().NotBeNull();
        company.Contact.Email.Should().Be("contact@keni.com");
        company.Contact.Phone.Should().Be("+351912345678");

        company.WizardStep
            .Should()
            .Be(CompanyWizardStep.FiscalInformation);

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

        var command = new CompleteContactInformationCommand
        {
            CompanyId = companyId,
            Email = "contact@keni.com",
            Phone = "+351912345678"
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
    public async Task Handle_ShouldCreateContactFromCommandValues()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var command = new CompleteContactInformationCommand
        {
            CompanyId = companyId,
            Email = "admin@keni.com",
            Phone = "+244923000000"
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
        company.Contact.Email
            .Should()
            .Be(command.Email);

        company.Contact.Phone
            .Should()
            .Be(command.Phone);
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

        var command = new CompleteContactInformationCommand
        {
            CompanyId = companyId,
            Email = "contact@keni.com",
            Phone = "+351912345678"
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
                    x.Contact.Email == command.Email &&
                    x.Contact.Phone == command.Phone &&
                    x.WizardStep == CompanyWizardStep.FiscalInformation)),
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

        var command = new CompleteContactInformationCommand
        {
            CompanyId = companyId,
            Email = "contact@keni.com",
            Phone = "+351912345678"
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