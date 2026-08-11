using AutoMapper;
using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Application.DTOs.Responses;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Application.Queries.GetCompanyDashboard;
using CompanyService.Core.Entities;
using CompanyService.Core.Exceptions;
using CompanyService.Core.ValueObjects;
using Core.Constants;
using FluentAssertions;
using Moq;
using Xunit;

namespace CompanyService.Tests.Application.Queries.TestsGetCompanyDashboard;

public sealed class GetCompanyDashboardQueryHandlerTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly GetCompanyDashboardQueryHandler _handler;

    public GetCompanyDashboardQueryHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetCompanyDashboardQueryHandler(
            _companyRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCompanyDashboardSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var company = Company.Register(
            userId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var companyId = company.Id;

        var expectedResponse = new CompanyDashboardResponse
        {
            CompanyId = company.Id,
            CompanyName = company.Name,
            Status = company.Status,
            WizardStep = company.WizardStep
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        _mapperMock
            .Setup(x => x.Map<CompanyDashboardResponse>(company))
            .Returns(expectedResponse);

        // Act
        var result = await _handler.Handle(
            new GetCompanyDashboardQuery(
                companyId,
                userId),
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        result.CompanyId
            .Should()
            .Be(company.Id);

        result.CompanyName
            .Should()
            .Be(company.Name);

        result.Status
            .Should()
            .Be(company.Status);

        result.WizardStep
            .Should()
            .Be(company.WizardStep);

        result.WizardCompleted
            .Should()
            .BeFalse();

        // Verifica que o acesso foi validado pelo utilizador
        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<CompanyDashboardResponse>(company),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCompanyDoesNotExist_ShouldThrowCompanyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company?)null);

        // Act
        Func<Task> act = () => _handler.Handle(
            new GetCompanyDashboardQuery(
                companyId,
                userId),
            CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<CompanyNotFoundException>();

        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<CompanyDashboardResponse>(
                It.IsAny<Company>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldMapCompanyToDashboardResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var company = Company.Register(
            userId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var companyId = company.Id;

        var expectedResponse = new CompanyDashboardResponse
        {
            CompanyId = company.Id,
            CompanyName = company.Name,
            Status = company.Status,
            WizardStep = company.WizardStep
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        _mapperMock
            .Setup(x => x.Map<CompanyDashboardResponse>(company))
            .Returns(expectedResponse);

        // Act
        var result = await _handler.Handle(
            new GetCompanyDashboardQuery(
                companyId,
                userId),
            CancellationToken.None);

        // Assert
        result.Should()
            .BeSameAs(expectedResponse);

        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<CompanyDashboardResponse>(company),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCompanyIsCompleted_ShouldReturnWizardCompletedAsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var company = Company.Register(
            userId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        company.CompleteBasicInformation("keni");

        company.CompleteContactInformation(
            new Contact(
                "contact@keni.com",
                "+351912345678"));

        company.CompleteFiscalInformation(
            "PT123456789",
            new Address(
                "Rua Principal",
                "Coimbra",
                "3000-000",
                "Portugal"));

        company.CompleteBranding(
            "Keni Restaurant",
            "https://keni.com/logo.png");

        var companyId = company.Id;

        var expectedResponse = new CompanyDashboardResponse
        {
            CompanyId = company.Id,
            CompanyName = company.Name,
            Status = company.Status,
            WizardStep = company.WizardStep
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        _mapperMock
            .Setup(x => x.Map<CompanyDashboardResponse>(company))
            .Returns(expectedResponse);

        // Act
        var result = await _handler.Handle(
            new GetCompanyDashboardQuery(
                companyId,
                userId),
            CancellationToken.None);

        // Assert
        result.Status
            .Should()
            .Be(CompanyStatus.Active);

        result.WizardStep
            .Should()
            .Be(CompanyWizardStep.Completed);

        result.WizardCompleted
            .Should()
            .BeTrue();

        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<CompanyDashboardResponse>(company),
            Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        // Arrange
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

        var companyId = company.Id;

        var response = new CompanyDashboardResponse
        {
            CompanyId = company.Id,
            CompanyName = company.Name,
            Status = company.Status,
            WizardStep = company.WizardStep
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                cancellationToken))
            .ReturnsAsync(company);

        _mapperMock
            .Setup(x => x.Map<CompanyDashboardResponse>(company))
            .Returns(response);

        // Act
        await _handler.Handle(
            new GetCompanyDashboardQuery(
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
    public async Task Handle_WhenCompanyDoesNotExist_ShouldNotInvokeMapper()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company?)null);

        // Act
        Func<Task> act = () =>
            _handler.Handle(
                new GetCompanyDashboardQuery(
                    companyId,
                    userId),
                CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<CompanyNotFoundException>();

        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<CompanyDashboardResponse>(
                It.IsAny<Company>()),
            Times.Never);
    }



}