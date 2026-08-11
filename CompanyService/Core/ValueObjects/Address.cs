namespace CompanyService.Core.ValueObjects;

public sealed class Address
{
    public string? Street { get; private set; }

    public string City { get; private set; } = string.Empty;

    public string? PostalCode { get; private set; }

    public string Country { get; private set; } = string.Empty;

    protected Address()
    {
    }

    public Address(
        string? street,
        string city,
        string? postalCode,
        string country)
    {
        Street = Normalize(street);
        City = city.Trim();
        PostalCode = Normalize(postalCode);
        Country = country.Trim();
    }

    public void Update(
        string street,
        string city,
        string postalCode,
        string country)
    {
        Street = Normalize(street);
        City = city.Trim();
        PostalCode = Normalize(postalCode);
        Country = country.Trim();
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    public override string ToString()
    {
        return $"{Street}, {PostalCode} {City}, {Country}";
    }
}