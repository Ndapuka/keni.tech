using CompanyService.API.Dtos.Requests.CompleteFiscalInformation;
using CompanyService.API.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CompanyService.Tests.API.Validators;

public sealed class CompleteFiscalInformationRequestValidatorTests
{
    private readonly CompleteFiscalInformationRequestValidator _validator = new();

    [Fact]
    public void Should_Validate_When_Request_Is_Valid()
    {
        var request = new CompleteFiscalInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
        };

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(nameof(CompleteFiscalInformationRequest.CompanyId))]
    [InlineData(nameof(CompleteFiscalInformationRequest.TaxNumber))]
    [InlineData(nameof(CompleteFiscalInformationRequest.Street))]
    [InlineData(nameof(CompleteFiscalInformationRequest.City))]
    [InlineData(nameof(CompleteFiscalInformationRequest.PostalCode))]
    [InlineData(nameof(CompleteFiscalInformationRequest.Country))]
    public void Should_Reject_Required_Empty_Fields(string propertyName)
    {
        var companyId = Guid.NewGuid();

        var request = propertyName switch
        {
            nameof(CompleteFiscalInformationRequest.CompanyId)
                => new CompleteFiscalInformationRequest
                {
                    CompanyId = Guid.Empty,
                    TaxNumber = "PT123456789",
                    Street = "Rua Principal",
                    City = "Coimbra",
                    PostalCode = "3000-000",
                    Country = "Portugal"
                },

            nameof(CompleteFiscalInformationRequest.TaxNumber)
                => new CompleteFiscalInformationRequest
                {
                    CompanyId = companyId,
                    TaxNumber = string.Empty,
                    Street = "Rua Principal",
                    City = "Coimbra",
                    PostalCode = "3000-000",
                    Country = "Portugal"
                },

            nameof(CompleteFiscalInformationRequest.Street)
                => new CompleteFiscalInformationRequest
                {
                    CompanyId = companyId,
                    TaxNumber = "PT123456789",
                    Street = string.Empty,
                    City = "Coimbra",
                    PostalCode = "3000-000",
                    Country = "Portugal"
                },

            nameof(CompleteFiscalInformationRequest.City)
                => new CompleteFiscalInformationRequest
                {
                    CompanyId = companyId,
                    TaxNumber = "PT123456789",
                    Street = "Rua Principal",
                    City = string.Empty,
                    PostalCode = "3000-000",
                    Country = "Portugal"
                },

            nameof(CompleteFiscalInformationRequest.PostalCode)
                => new CompleteFiscalInformationRequest
                {
                    CompanyId = companyId,
                    TaxNumber = "PT123456789",
                    Street = "Rua Principal",
                    City = "Coimbra",
                    PostalCode = string.Empty,
                    Country = "Portugal"
                },

            nameof(CompleteFiscalInformationRequest.Country)
                => new CompleteFiscalInformationRequest
                {
                    CompanyId = companyId,
                    TaxNumber = "PT123456789",
                    Street = "Rua Principal",
                    City = "Coimbra",
                    PostalCode = "3000-000",
                    Country = string.Empty
                },

            _ => throw new ArgumentOutOfRangeException(nameof(propertyName))
        };

        var result = _validator.TestValidate(request);

        result.Errors
            .Should()
            .Contain(x => x.PropertyName == propertyName);
    }

    [Fact]
    public void Should_Reject_TaxNumber_Exceeding_50_Characters()
    {
        var request = new CompleteFiscalInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            TaxNumber = new string('1', 51),
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.TaxNumber);
    }

    [Fact]
    public void Should_Reject_Empty_TaxNumber()
    {
        var request = new CompleteFiscalInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            TaxNumber = string.Empty,
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.TaxNumber);
    }

    [Fact]
    public void Should_Reject_Empty_Street()
    {
        var request = new CompleteFiscalInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            TaxNumber = "PT123456789",
            Street = string.Empty,
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Street);
    }

    [Fact]
    public void Should_Reject_Empty_City()
    {
        var request = new CompleteFiscalInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = string.Empty,
            PostalCode = "3000-000",
            Country = "Portugal"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.City);
    }

    [Fact]
    public void Should_Reject_Empty_PostalCode()
    {
        var request = new CompleteFiscalInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = string.Empty,
            Country = "Portugal"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.PostalCode);
    }

    [Fact]
    public void Should_Reject_Empty_Country()
    {
        var request = new CompleteFiscalInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = string.Empty
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Country);
    }

    private static CompleteFiscalInformationRequest CreateValidRequest()
    {
        return new CompleteFiscalInformationRequest
        {
            CompanyId = Guid.NewGuid(),
            TaxNumber = "PT123456789",
            Street = "Rua Principal",
            City = "Coimbra",
            PostalCode = "3000-000",
            Country = "Portugal"
        };
    }
}