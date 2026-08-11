using AutoMapper;
using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Application.DTOs.Responses;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Application.Queries.GetCurrentCompany;
using CompanyService.Core.Entities;
using CompanyService.Core.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CompanyService.Tests.Application.Queries.TestsGetCurrentCompany;

public sealed class GetCurrentCompanyQueryHandlerTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly GetCurrentCompanyQueryHandler _handler;

    public GetCurrentCompanyQueryHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetCurrentCompanyQueryHandler(
            _companyRepositoryMock.Object,
            _mapperMock.Object);
    }



    [Fact]
    public async Task Handle_ShouldReturnCurrentCompanySuccessfully()
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

        var expectedResponse = new CompanyResponse
        {
            CompanyId = company.Id,
            Name = company.Name,
            BusinessType = company.BusinessType,
            Status = company.Status.ToString(),
            WizardStep = company.WizardStep.ToString(),
            Country = company.Address.Country,
            City = company.Address.City
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        _mapperMock
            .Setup(x => x.Map<CompanyResponse>(company))
            .Returns(expectedResponse);

        // Act
        var result = await _handler.Handle(
            new GetCurrentCompanyQuery(
                companyId,
                userId),
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(expectedResponse);

        _companyRepositoryMock.Verify(
            x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<CompanyResponse>(company),
            Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldReturnMappedCompanyResponse()
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

        var expectedResponse = new CompanyResponse
        {
            CompanyId = company.Id,
            Name = company.Name,
            BusinessType = BusinessType.Restaurant,
            Status = company.Status.ToString(),
            WizardStep = company.WizardStep.ToString(),
            Country = "Portugal",
            City = "Coimbra"
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        _mapperMock
            .Setup(x => x.Map<CompanyResponse>(company))
            .Returns(expectedResponse);

        // Act
        var result = await _handler.Handle(
            new GetCurrentCompanyQuery(
                companyId,
                userId),
            CancellationToken.None);

        // Assert
        result.CompanyId
            .Should()
            .Be(company.Id);

        result.Name
            .Should()
            .Be(company.Name);

        result.BusinessType
            .Should()
            .Be(company.BusinessType);

        result.Status
            .Should()
            .Be(company.Status.ToString());

        result.WizardStep
            .Should()
            .Be(company.WizardStep.ToString());

        result.Country
            .Should()
            .Be(company.Address.Country);

        result.City
            .Should()
            .Be(company.Address.City);

        _mapperMock.Verify(
            x => x.Map<CompanyResponse>(company),
            Times.Once);
    }


    [Fact]
    public async Task Handle_WhenCompanyDoesNotExist_ShouldThrowCompanyNotFoundException()
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
        Func<Task> act = () =>
            _handler.Handle(
                new GetCurrentCompanyQuery(
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
            x => x.Map<CompanyResponse>(
                It.IsAny<Company>()),
            Times.Never);
    }



    [Fact]
    public async Task Handle_WhenCompanyDoesNotExist_ShouldNotInvokeMapper()
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
        Func<Task> act = () =>
            _handler.Handle(
                new GetCurrentCompanyQuery(
                    companyId,
                    userId),
                CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<CompanyNotFoundException>();

        _mapperMock.Verify(
            x => x.Map<CompanyResponse>(
                It.IsAny<Company>()),
            Times.Never);
    }



    [Fact]
    public async Task Handle_ShouldUseCompanyIdAndUserIdToFindCompany()
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

        var response = new CompanyResponse
        {
            CompanyId = company.Id,
            Name = company.Name,
            BusinessType = company.BusinessType,
            Status = company.Status.ToString(),
            WizardStep = company.WizardStep.ToString(),
            Country = company.Address.Country,
            City = company.Address.City
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        _mapperMock
            .Setup(x => x.Map<CompanyResponse>(company))
            .Returns(response);

        // Act
        await _handler.Handle(
            new GetCurrentCompanyQuery(
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

        _companyRepositoryMock.Verify(
            x => x.GetByOwnerUserIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
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

        var response = new CompanyResponse
        {
            CompanyId = company.Id,
            Name = company.Name,
            BusinessType = company.BusinessType,
            Status = company.Status.ToString(),
            WizardStep = company.WizardStep.ToString(),
            Country = company.Address.Country,
            City = company.Address.City
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdForMemberAsync(
                companyId,
                userId,
                cancellationToken))
            .ReturnsAsync(company);

        _mapperMock
            .Setup(x => x.Map<CompanyResponse>(company))
            .Returns(response);

        // Act
        await _handler.Handle(
            new GetCurrentCompanyQuery(
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
}
