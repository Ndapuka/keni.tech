using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.API.Dtos.Requests.InviteUser;
using CompanyService.API.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CompanyService.Tests.API.Validators;

public sealed class InviteUserRequestValidatorTests
{
    private readonly InviteUserRequestValidator _validator = new();

    [Fact]
    public void Should_Validate_When_Request_Is_Valid()
    {
        var request = new InviteUserRequest
        {

            UserId = Guid.NewGuid(),
            Role = CompanyRole.Manager
        };

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Reject_Empty_CompanyId()
    {
        var request = new InviteUserRequest
        {

            UserId = Guid.NewGuid(),
            Role = CompanyRole.Manager
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Should_Reject_Empty_UserId()
    {
        var request = new InviteUserRequest
        {

            UserId = Guid.Empty,
            Role = CompanyRole.Manager
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Should_Reject_Owner_Role()
    {
        var request = new InviteUserRequest
        {

            UserId = Guid.NewGuid(),
            Role = CompanyRole.Owner
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Fact]
    public void Should_Reject_Invalid_Role()
    {
        var request = new InviteUserRequest
        {

            UserId = Guid.NewGuid(),
            Role = (CompanyRole)999
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Theory]
    [InlineData(CompanyRole.Manager)]
    [InlineData(CompanyRole.Employee)]
    public void Should_Accept_Allowed_Roles(CompanyRole role)
    {
        var request = new InviteUserRequest
        {

            UserId = Guid.NewGuid(),
            Role = role
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Role);
    }
}