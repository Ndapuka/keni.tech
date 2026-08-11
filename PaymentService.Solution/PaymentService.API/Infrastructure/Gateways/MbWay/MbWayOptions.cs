namespace PaymentService.Infrastructure.Gateways.MbWay;

public sealed class MbWayOptions
{
    public const string SectionName = "PaymentProviders:MbWay";

    public string BaseUrl { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public string MerchantId { get; set; } = string.Empty;

    public string TerminalId { get; set; } = string.Empty;

    public string CallbackUrl { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 30;

    public bool UseSandbox { get; set; } = true;
}
