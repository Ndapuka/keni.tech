using AutoMapper;
using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.API.Controllers;
using CompanyService.API.Dtos.Requests.InviteUser;
using CompanyService.API.Dtos.Requests.RegisterCompany;
using CompanyService.API.Dtos.Requests.UpdateCompany;
using CompanyService.Application.Commands.InviteUser;
using CompanyService.Application.Commands.RegisterCompany;
using CompanyService.Application.Commands.UpdateCompany;
using CompanyService.Application.DTOs.Responses;
using CompanyService.Application.Queries.CheckCompanyMembership;
using CompanyService.Application.Queries.GetCompaniesQuery;
using CompanyService.Application.Queries.GetCompanyDashboard;
using CompanyService.Application.Queries.GetCompanyQuery;
using CompanyService.Application.Queries.GetCurrentCompany;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace CompanyService.Tests.API.Controllers;

public sealed class CompanyControllerTests
{
    private readonly Mock<ISender> _senderMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CompanyController _controller;

    public CompanyControllerTests()
    {
        _senderMock = new Mock<ISender>();
        _mapperMock = new Mock<IMapper>();

        _controller = new CompanyController(
            _senderMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Register_Should_Return_CreatedAtAction()
    {
        // Arrange
        var request = new RegisterCompanyRequest
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        var command = new RegisterCompanyCommand
        {
            OwnerUserId = request.OwnerUserId,
            Name = request.Name,
            BusinessType = request.BusinessType,
            Country = request.Country,
            City = request.City
        };

        var response = new RegisterCompanyResponse
        {
            CompanyId = Guid.NewGuid(),
            Status = "PendingConfiguration",
            WizardStep = "BasicInformation"
        };

        _mapperMock
            .Setup(x => x.Map<RegisterCompanyCommand>(request))
            .Returns(command);

        _senderMock
            .Setup(x => x.Send(
                command,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Register(
            request,
            CancellationToken.None);

        // Assert
        var created = result.Should()
            .BeOfType<CreatedAtActionResult>()
            .Subject;

        created.ActionName
            .Should()
            .Be(nameof(CompanyController.GetById));

        created.RouteValues
            .Should()
            .ContainKey("companyId");

        created.RouteValues!["companyId"]
            .Should()
            .Be(response.CompanyId);

        created.Value
            .Should()
            .Be(response);

        _mapperMock.Verify(
            x => x.Map<RegisterCompanyCommand>(request),
            Times.Once);

        _senderMock.Verify(
            x => x.Send(
                command,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetById_Should_Return_Ok_With_Company()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var response = new CompanyResponse
        {
            CompanyId = companyId,
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Status = "Active",
            WizardStep = "Completed",
            Country = "Portugal",
            City = "Coimbra"
        };

        _senderMock
            .Setup(x => x.Send(
                It.Is<GetCompanyQuery>(
                    q => q.CompanyId == companyId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetById(
            companyId,
            CancellationToken.None);

        // Assert
        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        ok.Value
            .Should()
            .Be(response);

        _senderMock.Verify(
            x => x.Send(
                It.Is<GetCompanyQuery>(
                    q => q.CompanyId == companyId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetMyCompanies_Should_Return_Ok_With_Companies()
    {
        // Arrange
        var userId = Guid.NewGuid();

        IReadOnlyCollection<CompanyResponse> response =
        [
            new CompanyResponse
            {
                CompanyId = Guid.NewGuid(),
                Name = "Keni",
                BusinessType = BusinessType.Restaurant,
                Status = "Active",
                WizardStep = "Completed",
                Country = "Portugal",
                City = "Coimbra"
            },
            new CompanyResponse
            {
                CompanyId = Guid.NewGuid(),
                Name = "Empresa XPTO",
                BusinessType = BusinessType.Restaurant,
                Status = "PendingConfiguration",
                WizardStep = "BasicInformation",
                Country = "Angola",
                City = "Lubango"
            }
        ];

        SetAuthenticatedUser(userId);

        _senderMock
            .Setup(x => x.Send(
                It.Is<GetCompaniesQuery>(
                    q => q.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetMyCompanies(
            CancellationToken.None);

        // Assert
        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        ok.Value
            .Should()
            .Be(response);

        _senderMock.Verify(
            x => x.Send(
                It.Is<GetCompaniesQuery>(
                    q => q.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetMyCompanies_Should_Throw_When_User_Identifier_Is_Missing()
    {
        // Arrange
        SetAuthenticatedUser(null);

        // Act
        var act = () => _controller.GetMyCompanies(
            CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetCurrent_Should_Return_Ok_With_Current_Company()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var activeCompanyId = Guid.NewGuid();

        var response = new CompanyResponse
        {
            CompanyId = activeCompanyId,
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Status = "Active",
            WizardStep = "Completed",
            Country = "Portugal",
            City = "Coimbra"
        };

        SetAuthenticatedUser(
            userId,
            activeCompanyId);

        _senderMock
            .Setup(x => x.Send(
                It.Is<GetCurrentCompanyQuery>(
                    q => q.CompanyId == activeCompanyId &&
                         q.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetCurrent(
            CancellationToken.None);

        // Assert
        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        ok.Value
            .Should()
            .Be(response);

        _senderMock.Verify(
            x => x.Send(
                It.Is<GetCurrentCompanyQuery>(
                    q => q.CompanyId == activeCompanyId &&
                         q.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCurrent_Should_Return_NoContent_When_ActiveCompany_Is_Missing()
    {
        // Arrange
        SetAuthenticatedUser(Guid.NewGuid());

        // Act
        var result = await _controller.GetCurrent(
            CancellationToken.None);

        // Assert
        result.Should()
            .BeOfType<NoContentResult>();

        _senderMock.Verify(
            x => x.Send(
                It.IsAny<GetCurrentCompanyQuery>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetCurrent_Should_Throw_When_User_Identifier_Is_Missing()
    {
        // Arrange
        var activeCompanyId = Guid.NewGuid();

        SetAuthenticatedUser(
            null,
            activeCompanyId);

        // Act
        var act = () => _controller.GetCurrent(
            CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<UnauthorizedAccessException>();

        _senderMock.Verify(
            x => x.Send(
                It.IsAny<GetCurrentCompanyQuery>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetCurrent_Should_Throw_When_User_Identifier_Is_Invalid()
    {
        // Arrange
        var activeCompanyId = Guid.NewGuid();

        SetAuthenticatedUser(
            "invalid-guid",
            activeCompanyId);

        // Act
        var act = () => _controller.GetCurrent(
            CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<UnauthorizedAccessException>();

        _senderMock.Verify(
            x => x.Send(
                It.IsAny<GetCurrentCompanyQuery>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetDashboard_Should_Return_Ok_With_Dashboard()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();

        var response = new CompanyDashboardResponse
        {
            CompanyId = companyId,
            CompanyName = "Keni",
            Status = CompanyStatus.Active,
            WizardStep = CompanyWizardStep.Completed
        };

        SetAuthenticatedUser(userId);

        _senderMock
            .Setup(x => x.Send(
                It.Is<GetCompanyDashboardQuery>(
                    q => q.CompanyId == companyId &&
                         q.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetDashboard(
            companyId,
            CancellationToken.None);

        // Assert
        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        ok.Value
            .Should()
            .Be(response);

        _senderMock.Verify(
            x => x.Send(
                It.Is<GetCompanyDashboardQuery>(
                    q => q.CompanyId == companyId &&
                         q.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetDashboard_Should_Throw_When_User_Identifier_Is_Missing()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        SetAuthenticatedUser(null);

        // Act
        var act = () => _controller.GetDashboard(
            companyId,
            CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<UnauthorizedAccessException>();

        _senderMock.Verify(
            x => x.Send(
                It.IsAny<GetCompanyDashboardQuery>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }


    [Fact]
    public async Task Update_Should_Return_NoContent()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var request = new UpdateCompanyRequest
        {
            CompanyId = Guid.NewGuid(), // valor irrelevante — a rota manda, não o corpo
            Name = "Keni Updated",
            BusinessType = BusinessType.Restaurant
        };

        var mappedCommand = new UpdateCompanyCommand
        {
            CompanyId = request.CompanyId,
            Name = request.Name,
            BusinessType = request.BusinessType
        };

        SetAuthenticatedUser(userId);

        _mapperMock
            .Setup(x => x.Map<UpdateCompanyCommand>(request))
            .Returns(mappedCommand);

        _senderMock
            .Setup(x => x.Send(
                It.Is<UpdateCompanyCommand>(c =>
                    c.CompanyId == companyId &&
                    c.UserId == userId &&
                    c.Name == request.Name &&
                    c.BusinessType == request.BusinessType),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.Update(
            companyId,
            request,
            CancellationToken.None);

        // Assert
        result.Should()
            .BeOfType<NoContentResult>();

        _mapperMock.Verify(
            x => x.Map<UpdateCompanyCommand>(request),
            Times.Once);

        _senderMock.Verify(
            x => x.Send(
                It.Is<UpdateCompanyCommand>(c =>
                    c.CompanyId == companyId &&
                    c.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }





    [Fact]
    public async Task IsActiveMember_Should_ReturnOk_When_UserIsActiveMember()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _senderMock
            .Setup(x => x.Send(
                It.Is<CheckCompanyMembershipQuery>(
                    q => q.CompanyId == companyId &&
                         q.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.IsActiveMember(
            companyId,
            userId,
            CancellationToken.None);

        // Assert
        var okResult = result
            .Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value
            .Should()
            .BeEquivalentTo(new
            {
                IsActiveMember = true
            });

        _senderMock.Verify(
            x => x.Send(
                It.Is<CheckCompanyMembershipQuery>(
                    q => q.CompanyId == companyId &&
                         q.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task IsActiveMember_Should_ReturnOk_When_UserIsNotActiveMember()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _senderMock
            .Setup(x => x.Send(
                It.Is<CheckCompanyMembershipQuery>(
                    q => q.CompanyId == companyId &&
                         q.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.IsActiveMember(
            companyId,
            userId,
            CancellationToken.None);

        // Assert
        var okResult = result
            .Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value
            .Should()
            .BeEquivalentTo(new
            {
                IsActiveMember = false
            });

        _senderMock.Verify(
            x => x.Send(
                It.Is<CheckCompanyMembershipQuery>(
                    q => q.CompanyId == companyId &&
                         q.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task IsActiveMember_Should_PassCancellationToken()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        _senderMock
            .Setup(x => x.Send(
                It.Is<CheckCompanyMembershipQuery>(
                    q => q.CompanyId == companyId &&
                         q.UserId == userId),
                cancellationToken))
            .ReturnsAsync(true);

        // Act
        await _controller.IsActiveMember(
            companyId,
            userId,
            cancellationToken);

        // Assert
        _senderMock.Verify(
            x => x.Send(
                It.Is<CheckCompanyMembershipQuery>(
                    q => q.CompanyId == companyId &&
                         q.UserId == userId),
                cancellationToken),
            Times.Once);
    }



    [Fact]
    public async Task IsActiveMember_Should_Return_Ok_When_User_Is_Not_Active_Member()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _senderMock
            .Setup(x => x.Send(
                It.Is<CheckCompanyMembershipQuery>(
                    q => q.CompanyId == companyId &&
                         q.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.IsActiveMember(
            companyId,
            userId,
            CancellationToken.None);

        // Assert
        var ok = result
            .Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        ok.Value.Should()
            .BeEquivalentTo(new
            {
                IsActiveMember = false
            });

        _senderMock.Verify(
            x => x.Send(
                It.Is<CheckCompanyMembershipQuery>(
                    q => q.CompanyId == companyId &&
                         q.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }





    [Fact]
    public async Task InviteUser_Should_Return_NoContent()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var request = new InviteUserRequest
        {

            UserId = userId,
            Role = CompanyRole.Manager
        };

        var command = new InviteUserCommand
        {
            CompanyId = companyId,
            UserId = userId,
            Role = request.Role
        };

        _mapperMock
            .Setup(x => x.Map<InviteUserCommand>(request))
            .Returns(command);

        _senderMock
            .Setup(x => x.Send(
                command,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(userId);

        // Act
        var result = await _controller.InviteUser(
            companyId,
            request,
            CancellationToken.None);

        // Assert
        result.Should()
            .BeOfType<NoContentResult>();

        _mapperMock.Verify(
            x => x.Map<InviteUserCommand>(request),
            Times.Once);

        _senderMock.Verify(
            x => x.Send(
                command,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private void SetAuthenticatedUser(Guid userId)
    {
        SetAuthenticatedUser(
            userId.ToString(),
            null);
    }

    private void SetAuthenticatedUser(
        Guid userId,
        Guid activeCompanyId)
    {
        SetAuthenticatedUser(
            userId.ToString(),
            activeCompanyId);
    }

    private void SetAuthenticatedUser(
        string? subject,
        Guid? activeCompanyId = null)
    {
        var claims = new List<Claim>();

        if (subject is not null)
        {
            claims.Add(
                new Claim("sub", subject));
        }

        if (activeCompanyId.HasValue)
        {
            claims.Add(
                new Claim(
                    "companyId",
                    activeCompanyId.Value.ToString()));
        }

        var identity = new ClaimsIdentity(
            claims,
            authenticationType: "Test");

        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };
    }
}
