using CompanyService.API.Dtos.Requests.CompleteBasicInformation;
using CompanyService.API.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CompanyService.Tests.API.Validators;

public sealed class CompleteBasicInformationRequestValidatorTests
{
    private readonly CompleteBasicInformationRequestValidator _validator = new();

    [Fact]
    public void Should_Validate_When_Request_Is_Valid()
    {
        var request = new CompleteBasicInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Slug = "keni-restaurant"
        };

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Empty_CompanyId()
    {
        var request = new CompleteBasicInformationRequest
        {
            CompanyId = Guid.Empty,
            Slug = "keni"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.CompanyId);
    }

    [Fact]
    public void Should_Reject_Empty_Slug()
    {
        var request = new CompleteBasicInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Slug = string.Empty
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Theory]
    [InlineData("Keni")]
    [InlineData("KENI")]
    [InlineData("keni_restaurant")]
    [InlineData("keni restaurant")]
    [InlineData("keni--restaurant")]
    [InlineData("-keni")]
    [InlineData("keni-")]
    public void Should_Reject_Invalid_Slug_Format(string slug)
    {
        var request = new CompleteBasicInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Slug = slug
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Fact]
    public void Should_Reject_Slug_Exceeding_150_Characters()
    {
        var request = new CompleteBasicInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Slug = new string('a', 151)
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Theory]
    [InlineData("keni")]
    [InlineData("keni123")]
    [InlineData("keni-restaurant")]
    [InlineData("keni-restaurant-2026")]
    public void Should_Accept_Valid_Slug_Format(string slug)
    {
        var request = new CompleteBasicInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Slug = slug
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Slug);
    }
}
