using CompanyService.Core.ValueObjects;
using Core.Constants;
using FluentAssertions;
using Xunit;

namespace CompanyService.Tests.Core.ValueObjects;

public sealed class ContactTests
{
    [Fact]
    public void Constructor_ShouldCreateContactWithProvidedValues()
    {
        // Arrange & Act
        var contact = new Contact(
            "contact@keni.com",
            "+351912345678");

        // Assert
        contact.Email.Should().Be("contact@keni.com");
        contact.Phone.Should().Be("+351912345678");
    }

    [Fact]
    public void Constructor_ShouldAllowEmptyValues()
    {
        // Arrange & Act
        var contact = new Contact(
            string.Empty,
            string.Empty);

        // Assert
        contact.Email.Should().BeEmpty();
        contact.Phone.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_ShouldPreserveProvidedValues()
    {
        // Arrange
        const string email = "admin@keni.com";
        const string phone = "+244923000000";

        // Act
        var contact = new Contact(
            email,
            phone);

        // Assert
        contact.Email.Should().Be(email);
        contact.Phone.Should().Be(phone);
    }
}
