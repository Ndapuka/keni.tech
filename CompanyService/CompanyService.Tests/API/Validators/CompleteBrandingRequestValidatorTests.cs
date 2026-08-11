using CompanyService.API.Dtos.Requests.CompleteBranding;
using CompanyService.API.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CompanyService.Tests.API.Validators;

public sealed class CompleteBrandingRequestValidatorTests
{
    private readonly CompleteBrandingRequestValidator _validator = new();

    [Fact]
    public void Should_Validate_When_Request_Is_Valid()
    {
        var request = new CompleteBrandingRequest
        {
            CompanyId = Guid.NewGuid(),
            Description = "Restaurant Keni",
            LogoUrl = "https://keni.com/logo.png"
        };

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Empty_CompanyId()
    {
        var request = new CompleteBrandingRequest
        {
            CompanyId = Guid.Empty
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.CompanyId);
    }

    [Fact]
    public void Should_Accept_Null_Description()
    {
        var request = new CompleteBrandingRequest
        {
            CompanyId = Guid.NewGuid(),
            Description = null
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Reject_Description_Exceeding_1000_Characters()
    {
        var request = new CompleteBrandingRequest
        {
            CompanyId = Guid.NewGuid(),
            Description = new string('a', 1001)
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Theory]
    [InlineData("https://keni.com/logo.png")]
    [InlineData("http://keni.com/logo.png")]
    public void Should_Accept_Valid_Http_Url(string logoUrl)
    {
        var request = new CompleteBrandingRequest
        {
            CompanyId = Guid.NewGuid(),
            LogoUrl = logoUrl
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.LogoUrl);
    }

    [Theory]
    [InlineData("ftp://keni.com/logo.png")]
    [InlineData("keni.com/logo.png")]
    [InlineData("not-a-url")]
    public void Should_Reject_Invalid_LogoUrl(string logoUrl)
    {
        var request = new CompleteBrandingRequest
        {
            CompanyId = Guid.NewGuid(),
            LogoUrl = logoUrl
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.LogoUrl);
    }

    [Fact]
    public void Should_Reject_LogoUrl_Exceeding_500_Characters()
    {
        var request = new CompleteBrandingRequest
        {
            CompanyId = Guid.NewGuid(),
            LogoUrl = $"https://keni.com/{new string('a', 500)}"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.LogoUrl);
    }
}