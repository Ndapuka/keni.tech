using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Core.Entities;
using CompanyService.Infrastructure.Persistence.Context;
using CompanyService.Infrastructure.Persistence.UnitOfWork;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CompanyService.Tests.Infrastruture.UnitOfWorkTests;

public sealed class UnitOfWorkTests
{
    private static CompanyDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<CompanyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CompanyDbContext(options);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistAddedCompany()
    {
        await using var context = CreateContext();

        var unitOfWork = new UnitOfWork(context);

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        await context.Companies.AddAsync(company);

        // Act
        var result = await unitOfWork.SaveChangesAsync();

        // Assert
        result.Should().BeGreaterThan(0);

        var persistedCompany = await context.Companies
            .FirstOrDefaultAsync(x => x.Id == company.Id);

        persistedCompany.Should().NotBeNull();
        persistedCompany!.Name.Should().Be("Keni");
        persistedCompany.BusinessType
            .Should()
            .Be(BusinessType.Restaurant);
        persistedCompany.Address.Country
            .Should()
            .Be("Portugal");
        persistedCompany.Address.City
            .Should()
            .Be("Coimbra");
    }

    [Fact]
    public async Task SaveChangesAsync_WhenThereAreNoChanges_ShouldReturnZero()
    {
        await using var context = CreateContext();

        var unitOfWork = new UnitOfWork(context);

        // Act
        var result = await unitOfWork.SaveChangesAsync();

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistMultipleCompanies()
    {
        await using var context = CreateContext();

        var unitOfWork = new UnitOfWork(context);

        var company1 = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        var company2 = Company.Register(
            Guid.NewGuid(),
            "XPTO",
            BusinessType.Retail,
            "Angola",
            "Lubango");

        await context.Companies.AddRangeAsync(
            company1,
            company2);

        // Act
        var result = await unitOfWork.SaveChangesAsync();

        // Assert
        result.Should().BeGreaterThan(0);

        var companies = await context.Companies
            .AsNoTracking()
            .ToListAsync();

        companies.Should().HaveCount(2);

        companies.Should().Contain(x =>
            x.Id == company1.Id &&
            x.Name == "Keni");

        companies.Should().Contain(x =>
            x.Id == company2.Id &&
            x.Name == "XPTO");
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistChanges_WithCancellationToken()
    {
        await using var context = CreateContext();

        var unitOfWork = new UnitOfWork(context);

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        await context.Companies.AddAsync(
            company,
            cancellationToken);

        // Act
        var result = await unitOfWork.SaveChangesAsync(
            cancellationToken);

        // Assert
        result.Should().BeGreaterThan(0);

        var persistedCompany = await context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == company.Id,
                cancellationToken);

        persistedCompany.Should().NotBeNull();
        persistedCompany!.Name.Should().Be("Keni");
    }
}