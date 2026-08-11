using CompanyService.API.Dtos.Requests.CompleteContactInformation;
using CompanyService.API.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CompanyService.Tests.API.Validators;

public sealed class CompleteContactInformationRequestValidatorTests
{
    private readonly CompleteContactInformationRequestValidator _validator = new();

    [Fact]
    public void Should_Validate_When_Request_Is_Valid()
    {
        var request = new CompleteContactInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Email = "contact@keni.com",
            Phone = "+351912345678"
        };

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Empty_CompanyId()
    {
        var request = new CompleteContactInformationRequest
        {
            CompanyId = Guid.Empty,
            Email = "contact@keni.com",
            Phone = "+351912345678"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.CompanyId);
    }

    [Fact]
    public void Should_Reject_Empty_Email()
    {
        var request = new CompleteContactInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Email = string.Empty,
            Phone = "+351912345678"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("@keni.com")]
    public void Should_Reject_Invalid_Email(string email)
    {
        var request = new CompleteContactInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Email = email,
            Phone = "+351912345678"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Reject_Email_Exceeding_254_Characters()
    {
        var request = new CompleteContactInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Email = $"{new string('a', 250)}@keni.com",
            Phone = "+351912345678"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Reject_Empty_Phone()
    {
        var request = new CompleteContactInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Email = "contact@keni.com",
            Phone = string.Empty
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Should_Reject_Phone_Exceeding_30_Characters()
    {
        var request = new CompleteContactInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            Email = "contact@keni.com",
            Phone = new string('1', 31)
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }
}