using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Core.Entities;
using CompanyService.Core.ValueObjects;
using Core.Constants;
using FluentAssertions;
using Xunit;

namespace CompanyService.Tests.Core.Entities;

public sealed class CompanyTests
{
    [Fact]
    public void Register_ShouldCreateCompanyWithInitialState()
    {
        // Arrange
        var ownerUserId = Guid.NewGuid();

        // Act
        var company = Company.Register(
            ownerUserId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Porto");

        // Assert
        company.Should().NotBeNull();

        company.OwnerUserId.Should().Be(ownerUserId);
        company.Name.Should().Be("Keni");
        company.BusinessType.Should().Be(BusinessType.Restaurant);

        company.Status
            .Should()
            .Be(CompanyStatus.PendingConfiguration);

        company.WizardStep
            .Should()
            .Be(CompanyWizardStep.BasicInformation);

        company.Users.Should().ContainSingle();

        var owner = company.Users.Single();

        owner.UserId.Should().Be(ownerUserId);
        owner.Role.Should().Be(CompanyRole.Owner);
        owner.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Register_ShouldInitializeAddress()
    {
        // Arrange
        var ownerUserId = Guid.NewGuid();

        // Act
        var company = Company.Register(
            ownerUserId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Porto");

        // Assert
        company.Address.Should().NotBeNull();
        company.Address.Country.Should().Be("Portugal");
        company.Address.City.Should().Be("Porto");
    }

    [Fact]
    public void CompleteBasicInformation_ShouldSetSlugAndAdvanceWizard()
    {
        // Arrange
        var company = CreateCompany();

        // Act
        company.CompleteBasicInformation("keni");

        // Assert
        company.Slug.Should().Be("keni");

        company.WizardStep
            .Should()
            .Be(CompanyWizardStep.ContactInformation);

        company.Status
            .Should()
            .Be(CompanyStatus.PendingConfiguration);
    }

    [Fact]
    public void CompleteContactInformation_ShouldSetContactAndAdvanceWizard()
    {
        // Arrange
        var company = CreateCompany();

        company.CompleteBasicInformation("keni");

        var contact = new Contact(
            "contact@keni.com",
            "+351912345678");

        // Act
        company.CompleteContactInformation(contact);

        // Assert
        company.Contact.Should().Be(contact);

        company.WizardStep
            .Should()
            .Be(CompanyWizardStep.FiscalInformation);

        company.Status
            .Should()
            .Be(CompanyStatus.PendingConfiguration);
    }

    [Fact]
    public void CompleteFiscalInformation_ShouldSetTaxNumberAndAddress()
    {
        // Arrange
        var company = CreateCompany();

        company.CompleteBasicInformation("keni");

        company.CompleteContactInformation(
            new Contact(
                "contact@keni.com",
                "+351912345678"));

        var address = new Address(
            "Rua Principal",
            "Coimbra",
            "3000-000",
            "Portugal");

        // Act
        company.CompleteFiscalInformation(
            "PT123456789",
            address);

        // Assert
        company.TaxNumber.Should().Be("PT123456789");
        company.Address.Should().Be(address);

        company.WizardStep
            .Should()
            .Be(CompanyWizardStep.Branding);

        company.Status
            .Should()
            .Be(CompanyStatus.PendingConfiguration);
    }

    [Fact]
    public void CompleteBranding_ShouldCompleteWizardAndActivateCompany()
    {
        // Arrange
        var company = CreateCompany();

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

        // Act
        company.CompleteBranding(
            "Restaurant management platform",
            "https://example.com/logo.png");

        // Assert
        company.Description
            .Should()
            .Be("Restaurant management platform");

        company.LogoUrl
            .Should()
            .Be("https://example.com/logo.png");

        company.WizardStep
            .Should()
            .Be(CompanyWizardStep.Completed);

        company.Status
            .Should()
            .Be(CompanyStatus.Active);
    }

    [Fact]
    public void CompleteBranding_WithNullOptionalFields_ShouldCompleteWizard()
    {
        // Arrange
        var company = CreateCompany();

        // Act
        company.CompleteBranding(
            null,
            null);

        // Assert
        company.Description.Should().BeNull();
        company.LogoUrl.Should().BeNull();

        company.WizardStep
            .Should()
            .Be(CompanyWizardStep.Completed);

        company.Status
            .Should()
            .Be(CompanyStatus.Active);
    }

    [Fact]
    public void UpdateName_ShouldChangeCompanyName()
    {
        // Arrange
        var company = CreateCompany();

        // Act
        company.UpdateName("Keni Updated");

        // Assert
        company.Name.Should().Be("Keni Updated");
    }

    [Fact]
    public void ChangeBusinessType_ShouldChangeBusinessType()
    {
        // Arrange
        var company = CreateCompany();

        // Act
        company.ChangeBusinessType(BusinessType.Barbershop);

        // Assert
        company.BusinessType
            .Should()
            .Be(BusinessType.Barbershop);
    }

    [Fact]
    public void InviteUser_ShouldAddUserToCompany()
    {
        // Arrange
        var company = CreateCompany();

        var userId = Guid.NewGuid();

        // Act
        company.InviteUser(
            userId,
            CompanyRole.Employee);

        // Assert
        company.Users.Should().HaveCount(2);

        company.Users
            .Should()
            .ContainSingle(x =>
                x.UserId == userId &&
                x.Role == CompanyRole.Employee &&
                x.IsActive);
    }

    [Fact]
    public void InviteUser_WhenUserAlreadyBelongsToCompany_ShouldThrow()
    {
        // Arrange
        var company = CreateCompany();

        var userId = Guid.NewGuid();

        company.InviteUser(
            userId,
            CompanyRole.Employee);

        // Act
        var action = () =>
            company.InviteUser(
                userId,
                CompanyRole.Manager);

        // Assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User already belongs to this company.");
    }



    [Fact]
    public void RemoveUser_WhenUserDoesNotExist_ShouldDoNothing()
    {
        // Arrange
        var company = CreateCompany();

        // Act
        var action = () =>
            company.RemoveUser(Guid.NewGuid());

        // Assert
        action.Should().NotThrow();

        company.Users.Should().ContainSingle();
    }

    [Fact]
    public void RemoveUser_WhenUserIsOwner_ShouldThrow()
    {
        // Arrange
        var ownerUserId = Guid.NewGuid();

        var company = Company.Register(
            ownerUserId,
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Porto");

        // Act
        var action = () =>
            company.RemoveUser(ownerUserId);

        // Assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("The owner cannot be removed.");
    }

    [Fact]
    public void Activate_ShouldSetCompanyAsActive()
    {
        // Arrange
        var company = CreateCompany();

        // Act
        company.Activate();

        // Assert
        company.Status
            .Should()
            .Be(CompanyStatus.Active);
    }

    [Fact]
    public void Suspend_ShouldSetCompanyAsSuspended()
    {
        // Arrange
        var company = CreateCompany();

        // Act
        company.Suspend();

        // Assert
        company.Status
            .Should()
            .Be(CompanyStatus.Suspended);
    }

    [Fact]
    public void Deactivate_ShouldSetCompanyAsInactive()
    {
        // Arrange
        var company = CreateCompany();

        // Act
        company.Deactivate();

        // Assert
        company.Status
            .Should()
            .Be(CompanyStatus.Inactive);
    }
    [Fact]
    public void RemoveUser_ShouldDeactivateNonOwnerUser()
    {
        // Arrange
        var company = CreateCompany();

        var userId = Guid.NewGuid();

        company.InviteUser(
            userId,
            CompanyRole.Employee);

        // Act
        company.RemoveUser(userId);

        // Assert
        var member = company.Users
            .Single(x => x.UserId == userId);

        member.IsActive.Should().BeFalse();
    }

    private static Company CreateCompany()
    {
        return Company.Register(
            Guid.NewGuid(),
            "Keni",
            BusinessType.Restaurant,
            "Portugal",
            "Porto");
    }
}