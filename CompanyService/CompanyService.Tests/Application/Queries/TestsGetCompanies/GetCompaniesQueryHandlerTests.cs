using AutoMapper;
using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Application.Interfaces.Persistence;
using CompanyService.Application.Queries.GetCompaniesQuery;
using CompanyService.Core.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace CompanyService.Tests.Application.Queries.TestsGetCompanies;

public sealed class GetCompaniesQueryHandlerTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly GetCompaniesQueryHandler _handler;

    public GetCompaniesQueryHandlerTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetCompaniesQueryHandler(
            _companyRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCompaniesForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var company1 = Company.Register(
            userId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var company2 = Company.Register(
            Guid.NewGuid(),
            "Empresa XPTO",
            BusinessType.Retail,
            "Angola",
            "Lubango");

        // O utilizador também pertence à segunda empresa
        company2.InviteUser(
            userId,
            CompanyRole.Employee);

        var companies = new List<Company>
    {
        company1,
        company2
    };

        _companyRepositoryMock
            .Setup(x => x.GetByMemberUserIdAsync(
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(companies);

        // Act
        var result = await _handler.Handle(
            new GetCompaniesQuery(userId),
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result.Should().ContainSingle(x =>
            x.CompanyId == company1.Id &&
            x.Name == company1.Name &&
            x.BusinessType == company1.BusinessType &&
            x.Status == company1.Status.ToString() &&
            x.WizardStep == company1.WizardStep.ToString() &&
            x.Country == company1.Address.Country &&
            x.City == company1.Address.City);

        result.Should().ContainSingle(x =>
            x.CompanyId == company2.Id &&
            x.Name == company2.Name &&
            x.BusinessType == company2.BusinessType &&
            x.Status == company2.Status.ToString() &&
            x.WizardStep == company2.WizardStep.ToString() &&
            x.Country == company2.Address.Country &&
            x.City == company2.Address.City);

        _companyRepositoryMock.Verify(
            x => x.GetByMemberUserIdAsync(
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenThereAreNoCompanies_ShouldReturnEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _companyRepositoryMock
            .Setup(x => x.GetByMemberUserIdAsync(
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Company>());

        // Act
        var result = await _handler.Handle(
            new GetCompaniesQuery(userId),
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _companyRepositoryMock.Verify(
            x => x.GetByMemberUserIdAsync(
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapCompanyPropertiesCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var companies = new List<Company>
        {
            company
        };

        _companyRepositoryMock
        .Setup(x => x.GetByMemberUserIdAsync(
            userId,
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(companies);

        // Act
        var result = await _handler.Handle(
            new GetCompaniesQuery(userId),
            CancellationToken.None);

        // Assert
        var response = result.Should()
            .ContainSingle()
            .Subject;

        response.CompanyId
            .Should()
            .Be(company.Id);

        response.Name
            .Should()
            .Be(company.Name);

        response.BusinessType
            .Should()
            .Be(company.BusinessType);

        response.Status
            .Should()
            .Be(company.Status.ToString());

        response.WizardStep
            .Should()
            .Be(company.WizardStep.ToString());

        response.Country
            .Should()
            .Be(company.Address.Country);

        response.City
            .Should()
            .Be(company.Address.City);
    }

    [Fact]
    public async Task Handle_ShouldReturnCompaniesInRepositoryOrder()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var company1 = Company.Register(
            Guid.NewGuid(),
            "Company A",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var company2 = Company.Register(
            Guid.NewGuid(),
            "Company B",
            BusinessType.Restaurant,
            "Portugal",
            "Porto");

        var company3 = Company.Register(
            Guid.NewGuid(),
            "Company C",
            BusinessType.Barbershop,
            "Angola",
            "Lubango");

        var companies = new List<Company>
        {
            company1,
            company2,
            company3
        };

        _companyRepositoryMock
        .Setup(x => x.GetByMemberUserIdAsync(
            userId,
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(companies);

        // Act
        var result = await _handler.Handle(
            new GetCompaniesQuery(userId),
            CancellationToken.None);

        // Assert
        result.Select(x => x.CompanyId)
            .Should()
            .ContainInOrder(
                company1.Id,
                company2.Id,
                company3.Id);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryOnlyOnce() //cs0161 
    {
        var userId = Guid.NewGuid();
        var company = Company.Register(
        userId,
        "Keni",
        BusinessType.Restaurant,
        "Portugal",
        "Coimbra");
        _companyRepositoryMock
        .Setup(x => x.GetByMemberUserIdAsync(
            userId,
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(new List<Company>
        {
            company
        });

        // Act
        await _handler.Handle(
            new GetCompaniesQuery(userId),
            CancellationToken.None);

        // Assert
        _companyRepositoryMock.Verify(
            x => x.GetAllAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        // Arrange
        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        var userId = Guid.NewGuid();

        var company = Company.Register(
            userId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        _companyRepositoryMock
            .Setup(x => x.GetByMemberUserIdAsync(
                userId,
                cancellationToken))
            .ReturnsAsync(new List<Company>
            {
            company
            });

        // Act
        await _handler.Handle(
            new GetCompaniesQuery(userId),
            cancellationToken);

        // Assert
        _companyRepositoryMock.Verify(
            x => x.GetByMemberUserIdAsync(
                userId,
                cancellationToken),
            Times.Once);
    }
}