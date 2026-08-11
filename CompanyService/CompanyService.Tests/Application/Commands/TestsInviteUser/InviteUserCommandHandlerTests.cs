using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Application.Commands.InviteUser;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Core.Entities;
using CompanyService.Core.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CompanyService.Tests.Application.Commands.TestsInviteUser;

public sealed class InviteUserCommandHandlerTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<InviteUserCommandHandler>> _loggerMock;

    private readonly InviteUserCommandHandler _handler;

    public InviteUserCommandHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock =
            new Mock<ILogger<InviteUserCommandHandler>>();

        _handler = new InviteUserCommandHandler(
            _companyRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldInviteUserSuccessfully()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var command = new InviteUserCommand
        {
            CompanyId = companyId,
            UserId = userId,
            Role = CompanyRole.Employee
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
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.Should().Be(userId);

        company.Users.Should().ContainSingle(
            x =>
                x.UserId == userId &&
                x.Role == CompanyRole.Employee &&
                x.IsActive);

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
        var userId = Guid.NewGuid();

        var command = new InviteUserCommand
        {
            CompanyId = companyId,
            UserId = userId,
            Role = CompanyRole.Employee
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
    public async Task Handle_ShouldAssignRequestedRoleToUser()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var command = new InviteUserCommand
        {
            CompanyId = companyId,
            UserId = userId,
            Role = CompanyRole.Manager
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(
                companyId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.Should().Be(userId);

        var invitedUser = company.Users
            .Single(x => x.UserId == userId);

        invitedUser.Role
            .Should()
            .Be(CompanyRole.Manager);

        invitedUser.IsActive
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task Handle_WhenUserAlreadyBelongsToCompany_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        company.InviteUser(
            userId,
            CompanyRole.Employee);

        var command = new InviteUserCommand
        {
            CompanyId = companyId,
            UserId = userId,
            Role = CompanyRole.Manager
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(
                companyId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        // Act
        var act = () =>
            _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("User already belongs to this company.");

        _companyRepositoryMock.Verify(
            x => x.Update(It.IsAny<Company>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldPersistUpdatedCompany()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var command = new InviteUserCommand
        {
            CompanyId = companyId,
            UserId = userId,
            Role = CompanyRole.Employee
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
                    x.Users.Any(user =>
                        user.UserId == userId &&
                        user.Role == CompanyRole.Employee &&
                        user.IsActive))),
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
        var userId = Guid.NewGuid();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        using var cancellationTokenSource = new CancellationTokenSource();

        var cancellationToken = cancellationTokenSource.Token;

        var command = new InviteUserCommand
        {
            CompanyId = companyId,
            UserId = userId,
            Role = CompanyRole.Employee
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
