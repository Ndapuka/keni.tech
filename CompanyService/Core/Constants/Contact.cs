namespace Core.Constants;

public sealed class Contact
{
    public string? Email { get; private set; }

    public string? Phone { get; private set; }

    protected Contact() // new protected member
    {
    }

    public Contact(
        string? email,
        string? phone)
    {
        Email = Normalize(email);
        Phone = Normalize(phone);
    }

    public void Update(
        string email,
        string phone)
    {
        Email = Normalize(email);
        Phone = Normalize(phone);
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    public override string ToString()
    {
        return $"{Email ?? "-"} | {Phone ?? "-"}";
    }
}