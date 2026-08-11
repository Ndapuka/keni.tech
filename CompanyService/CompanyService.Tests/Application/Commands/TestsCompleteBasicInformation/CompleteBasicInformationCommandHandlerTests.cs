using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Application.Commands.CompleteBasicInformation;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Entities;
using CompanyService.Core.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CompanyService.Tests.Application.Commands.TestsCompleteBasicInformation;

public sealed class CompleteBasicInformationCommandHandlerTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CompleteBasicInformationCommandHandler>> _loggerMock;

    private readonly CompleteBasicInformationCommandHandler _handler;

    public CompleteBasicInformationCommandHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock =
            new Mock<ILogger<CompleteBasicInformationCommandHandler>>();

        _handler = new CompleteBasicInformationCommandHandler(
            _companyRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCompleteBasicInformationSuccessfully()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var command = new CompleteBasicInformationCommand
        {
            CompanyId = companyId,
            Slug = "keni"
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
        company.Slug.Should().Be("keni");

        company.WizardStep
            .Should()
            .Be(CompanyWizardStep.ContactInformation);

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

        var command = new CompleteBasicInformationCommand
        {
            CompanyId = companyId,
            Slug = "keni"
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

        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var command = new CompleteBasicInformationCommand
        {
            CompanyId = companyId,
            Slug = "keni"
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

        var command = new CompleteBasicInformationCommand
        {
            CompanyId = companyId,
            Slug = "keni"
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
                    x.Slug == "keni" &&
                    x.WizardStep == CompanyWizardStep.ContactInformation)),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
