using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.API.Dtos.Requests.UpdateCompany;
using CompanyService.API.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CompanyService.Tests.API.Validators;

public sealed class UpdateCompanyRequestValidatorTests
{
    private readonly UpdateCompanyRequestValidator _validator = new();

    [Fact]
    public void Should_Validate_When_Request_Is_Valid()
    {
        var request = new UpdateCompanyRequest
        {
            CompanyId = Guid.NewGuid(),
            Name = "Keni Updated",
            BusinessType = BusinessType.Restaurant
        };

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Empty_CompanyId()
    {
        var request = new UpdateCompanyRequest
        {
            CompanyId = Guid.Empty,
            Name = "Keni Updated",
            BusinessType = BusinessType.Restaurant
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.CompanyId);
    }

    [Fact]
    public void Should_Reject_Empty_Name()
    {
        var request = new UpdateCompanyRequest
        {
            CompanyId = Guid.NewGuid(),
            Name = string.Empty,
            BusinessType = BusinessType.Restaurant
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Reject_Name_Exceeding_200_Characters()
    {
        var request = new UpdateCompanyRequest
        {
            CompanyId = Guid.NewGuid(),
            Name = new string('a', 201),
            BusinessType = BusinessType.Restaurant
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Reject_Invalid_BusinessType()
    {
        var request = new UpdateCompanyRequest
        {
            CompanyId = Guid.NewGuid(),
            Name = "Keni Updated",
            BusinessType = (BusinessType)999
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.BusinessType);
    }
}