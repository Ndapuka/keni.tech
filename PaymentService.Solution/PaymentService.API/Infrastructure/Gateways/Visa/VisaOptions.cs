namespace PaymentService.Infrastructure.Gateways.Visa;

public sealed class VisaOptions
{
    public const string SectionName = "PaymentProviders:Visa";

    public string BaseUrl { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public string MerchantId { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;

    public string CallbackUrl { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 30;

    public bool UseSandbox { get; set; } = true;
}
