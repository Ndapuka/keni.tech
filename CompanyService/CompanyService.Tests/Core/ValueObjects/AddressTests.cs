using CompanyService.Core.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CompanyService.Tests.Core.ValueObjects;

public sealed class AddressTests
{
    [Fact]
    public void Constructor_ShouldCreateAddressWithProvidedValues()
    {
        // Arrange & Act
        var address = new Address(
            "Rua Principal",
            "Coimbra",
            "3000-000",
            "Portugal");

        // Assert
        address.Street.Should().Be("Rua Principal");
        address.City.Should().Be("Coimbra");
        address.PostalCode.Should().Be("3000-000");
        address.Country.Should().Be("Portugal");
    }

    [Fact]
    public void Constructor_ShouldAllowEmptyValues()
    {
        // Arrange & Act
        var address = new Address(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty);

        // Assert
        address.Street.Should().BeEmpty();
        address.City.Should().BeEmpty();
        address.PostalCode.Should().BeEmpty();
        address.Country.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_ShouldPreserveProvidedValues()
    {
        // Arrange
        const string street = "Rua da Sofia";
        const string city = "Coimbra";
        const string postalCode = "3000-390";
        const string country = "Portugal";

        // Act
        var address = new Address(
            street,
            city,
            postalCode,
            country);

        // Assert
        address.Street.Should().Be(street);
        address.City.Should().Be(city);
        address.PostalCode.Should().Be(postalCode);
        address.Country.Should().Be(country);
    }
}