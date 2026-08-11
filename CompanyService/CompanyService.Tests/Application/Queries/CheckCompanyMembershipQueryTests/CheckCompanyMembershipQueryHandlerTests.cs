using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Application.Queries.CheckCompanyMembership;
using CompanyService.Core.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace CompanyService.Tests.Application.Queries.CheckCompanyMembership;

public sealed class CheckCompanyMembershipQueryHandlerTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly CheckCompanyMembershipQueryHandler _handler;

    public CheckCompanyMembershipQueryHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();

        _handler = new CheckCompanyMembershipQueryHandler(
            _companyRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserIsActiveMember_ShouldReturnTrue()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var company = Company.Register(
            userId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        // Act
        var result = await _handler.Handle(
            new CheckCompanyMembershipQuery(
                companyId,
                userId),
            CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenUserIsActiveMember()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var company = Company.Register(
            userId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        // Act
        var result = await _handler.Handle(
            new CheckCompanyMembershipQuery(
                companyId,
                userId),
            CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenUserIsNotMember()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company?)null);

        // Act
        var result = await _handler.Handle(
            new CheckCompanyMembershipQuery(
                companyId,
                userId),
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCompanyIdAndUserIdToRepository()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var company = Company.Register(
            userId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        // Act
        await _handler.Handle(
            new CheckCompanyMembershipQuery(
                companyId,
                userId),
            CancellationToken.None);

        // Assert
        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        var company = Company.Register(
            userId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                cancellationToken))
            .ReturnsAsync(company);

        // Act
        await _handler.Handle(
            new CheckCompanyMembershipQuery(
                companyId,
                userId),
            cancellationToken);

        // Assert
        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryOnlyOnce()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var company = Company.Register(
            userId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        // Act
        await _handler.Handle(
            new CheckCompanyMembershipQuery(
                companyId,
                userId),
            CancellationToken.None);

        // Assert
        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}