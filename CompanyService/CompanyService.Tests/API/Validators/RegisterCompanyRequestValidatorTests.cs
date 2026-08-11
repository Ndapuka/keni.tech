using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.API.Dtos.Requests.RegisterCompany;
using CompanyService.API.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CompanyService.Tests.API.Validators;

public sealed class RegisterCompanyRequestValidatorTests
{
    private readonly RegisterCompanyRequestValidator _validator = new();

    [Fact]
    public void Should_Validate_When_Request_Is_Valid()
    {
        var request = new RegisterCompanyRequest
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Empty_OwnerUserId()
    {
        var request = new RegisterCompanyRequest
        {
            OwnerUserId = Guid.Empty,
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.OwnerUserId);
    }

    [Fact]
    public void Should_Reject_Empty_Name()
    {
        var request = new RegisterCompanyRequest
        {
            OwnerUserId = Guid.NewGuid(),
            Name = string.Empty,
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Reject_Name_Exceeding_200_Characters()
    {
        var request = new RegisterCompanyRequest
        {
            OwnerUserId = Guid.NewGuid(),
            Name = new string('a', 201),
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = "Coimbra"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Reject_Invalid_BusinessType()
    {
        var request = new RegisterCompanyRequest
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = (BusinessType)999,
            Country = "Portugal",
            City = "Coimbra"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.BusinessType);
    }

    [Fact]
    public void Should_Reject_Empty_Country()
    {
        var request = new RegisterCompanyRequest
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = string.Empty,
            City = "Coimbra"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Country);
    }

    [Fact]
    public void Should_Reject_Country_Exceeding_100_Characters()
    {
        var request = new RegisterCompanyRequest
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = new string('a', 101),
            City = "Coimbra"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Country);
    }

    [Fact]
    public void Should_Reject_Empty_City()
    {
        var request = new RegisterCompanyRequest
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = string.Empty
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.City);
    }

    [Fact]
    public void Should_Reject_City_Exceeding_100_Characters()
    {
        var request = new RegisterCompanyRequest
        {
            OwnerUserId = Guid.NewGuid(),
            Name = "Keni",
            BusinessType = BusinessType.Restaurant,
            Country = "Portugal",
            City = new string('a', 101)
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.City);
    }
}