using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Core.Entities;
using CompanyService.Core.ValueObjects;
using CompanyService.Infrastructure.Persistence.Context;
using CompanyService.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CompanyService.Tests.Infrastructure.Persistence;

public sealed class CompanyRepositoryTests
{
    private static CompanyDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<CompanyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CompanyDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCompany()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

        var ownerUserId = Guid.NewGuid();

        var company = Company.Register(
            ownerUserId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        // Act
        await repository.AddAsync(company);

        await context.SaveChangesAsync();

        // Assert
        var result = await context.Companies
            .FirstOrDefaultAsync(x => x.Id == company.Id);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Keni");
        result.OwnerUserId.Should().Be(ownerUserId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCompanyWithUsers()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

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
        var result = await repository.GetByIdAsync(company.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(company.Id);
        result.Name.Should().Be("Keni");

        result.Users.Should().NotBeNull();
        result.Users.Should().ContainSingle();

        result.Users.First().UserId
            .Should()
            .Be(ownerUserId);

        result.Users.First().Role
            .Should()
            .Be(CompanyRole.Owner);
    }


    [Fact]
    public async Task GetByIdAsync_WhenCompanyDoesNotExist_ShouldReturnNull()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByOwnerIdAsync_ShouldReturnCompany()
    {
        // Arrange
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

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
        // Verifica se o repository consegue localizar a empresa
        // através do identificador do utilizador proprietário.
        var result = await repository.GetByOwnerUserIdAsync(ownerUserId);

        // Assert
        // Garante que a empresa foi encontrada e que pertence
        // efetivamente ao ownerUserId utilizado na consulta.
        result.Should().NotBeNull();
        result!.Id.Should().Be(company.Id);
        result.OwnerUserId.Should().Be(ownerUserId);
    }

    [Fact]
    public async Task GetByOwnerUserIdAsync_ShouldReturnCompany()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

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
        var result = await repository.GetByOwnerUserIdAsync(ownerUserId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(company.Id);
    }


    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCompaniesOrderedByName()
    {
        // Arrange
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

        var companyZeta = Company.Register(
            Guid.NewGuid(),
            "Zeta",
            BusinessType.Restaurant,
            "Portugal",
            "Porto");

        var companyAlpha = Company.Register(
            Guid.NewGuid(),
            "Alpha",
            BusinessType.Retail,
            "Portugal",
            "Coimbra");

        var companyKeni = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Barbershop,
            "Angola",
            "Lubango");

        // Inserimos deliberadamente numa ordem diferente
        // daquela que esperamos receber do repositório.
        await context.Companies.AddRangeAsync(
            companyZeta,
            companyAlpha,
            companyKeni);

        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync(
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        // O repositório deve ordenar as empresas alfabeticamente pelo Name.
        result.Select(x => x.Name)
            .Should()
            .Equal(
                "Alpha",
                "Keni",
                "Zeta");

        // Garante que as entidades devolvidas são exatamente
        // as empresas que foram persistidas.
        result.Select(x => x.Id)
            .Should()
            .ContainInOrder(
                companyAlpha.Id,
                companyKeni.Id,
                companyZeta.Id);
    }








    [Fact]
    public async Task GetAllAsync_WhenThereAreNoCompanies_ShouldReturnEmptyCollection()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

        // Act
        var result = await repository.GetAllAsync(
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ExistsBySlugAsync_WhenSlugExists_ShouldReturnTrue()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        company.CompleteBasicInformation("keni");

        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsBySlugAsync("keni");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsBySlugAsync_WhenSlugDoesNotExist_ShouldReturnFalse()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

        var result = await repository.ExistsBySlugAsync("non-existent");

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsByTaxNumberAsync_WhenTaxNumberExists_ShouldReturnTrue()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        company.CompleteFiscalInformation(
            "PT123456789",
            new Address(
                "Rua A",
                "Coimbra",
                "3000-000",
                "Portugal"));

        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsByTaxNumberAsync(
            "PT123456789");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByTaxNumberAsync_WhenTaxNumberDoesNotExist_ShouldReturnFalse()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

        var result = await repository.ExistsByTaxNumberAsync(
            "PT999999999");

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsByOwnerAsync_WhenOwnerExists_ShouldReturnTrue()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

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
        var result = await repository.ExistsByOwnerAsync(
            ownerUserId,
            CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByOwnerAsync_WhenOwnerDoesNotExist_ShouldReturnFalse()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

        var result = await repository.ExistsByOwnerAsync(
            Guid.NewGuid(),
            CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task Update_ShouldUpdateCompany()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        company.UpdateName("Keni Updated");

        // Act
        repository.Update(company);

        await context.SaveChangesAsync();

        // Assert
        var result = await context.Companies
            .FirstAsync(x => x.Id == company.Id);

        result.Name.Should().Be("Keni Updated");
    }

    [Fact]
    public async Task Remove_ShouldRemoveCompany()
    {
        await using var context = CreateContext();

        var repository = new CompanyRepository(context);

        var company = Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Coimbra");

        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        // Act
        repository.Remove(company);

        await context.SaveChangesAsync();

        // Assert
        var result = await context.Companies
            .FirstOrDefaultAsync(x => x.Id == company.Id);

        result.Should().BeNull();
    }
}
