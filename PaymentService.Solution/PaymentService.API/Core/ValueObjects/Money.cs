namespace PaymentService.Core.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }

    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required.");

        Amount = decimal.Round(amount, 2, MidpointRounding.ToEven);

        Currency = currency.ToUpperInvariant();
    }

    public override string ToString()
        => $"{Amount} {Currency}";
}