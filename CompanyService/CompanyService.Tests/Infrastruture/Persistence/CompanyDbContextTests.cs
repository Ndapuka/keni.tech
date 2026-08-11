using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Core.Entities;
using CompanyService.Core.ValueObjects;
using CompanyService.Infrastructure.Persistence.Context;
using Core.Constants;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CompanyService.Tests.Infrastructure.Persistence;

public sealed class CompanyDbContextTests
{
    private static CompanyDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<CompanyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CompanyDbContext(options);
    }

    [Fact]
    public void DbContext_ShouldExposeCompaniesDbSet()
    {
        using var context = CreateContext();

        context.Companies.Should().NotBeNull();
    }

    [Fact]
    public void DbContext_ShouldExposeCompanyUsersDbSet()
    {
        using var context = CreateContext();

        context.CompanyUsers.Should().NotBeNull();
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldSetCreatedAtForNewCompany()
    {
        await using var context = CreateContext();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        await context.Companies.AddAsync(company);

        // Act
        await context.SaveChangesAsync();

        // Assert
        company.CreatedAt
            .Should()
            .BeCloseTo(
                DateTime.UtcNow,
                TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldSetUpdatedAtWhenEntityIsModified()
    {
        await using var context = CreateContext();

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        // Act
        company.UpdateName("Keni Updated");

        await context.SaveChangesAsync();

        // Assert
        company.UpdatedAt.Should().NotBeNull();

        company.UpdatedAt!.Value
            .Should()
            .BeCloseTo(
                DateTime.UtcNow,
                TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistCompanyUsers()
    {
        await using var context = CreateContext();

        var ownerUserId = Guid.NewGuid();

        var company = Company.Register(
            ownerUserId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        // Act
        var persistedUsers = await context.CompanyUsers
            .Where(x => x.CompanyId == company.Id)
            .ToListAsync();

        // Assert
        persistedUsers.Should().ContainSingle();

        var owner = persistedUsers.Single();

        owner.UserId.Should().Be(ownerUserId);
        owner.Role.Should().Be(CompanyRole.Owner);
        owner.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistCompleteCompanyWizardState()
    {
        await using var context = CreateContext();

        var company = Company.Register(
            Guid.NewGuid(),
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

        await context.Companies.AddAsync(company);

        // Act
        await context.SaveChangesAsync();

        // Assert
        var result = await context.Companies
            .AsNoTracking()
            .FirstAsync(x => x.Id == company.Id);

        result.Name.Should().Be("Keni");
        result.Slug.Should().Be("keni");
        result.TaxNumber.Should().Be("PT123456789");

        result.Status
            .Should()
            .Be(CompanyStatus.Active);

        result.WizardStep
            .Should()
            .Be(CompanyWizardStep.Completed);

        result.Address.Country
            .Should()
            .Be("Portugal");

        result.Address.City
            .Should()
            .Be("Coimbra");

        result.Contact.Email
            .Should()
            .Be("contact@keni.com");
    }
}