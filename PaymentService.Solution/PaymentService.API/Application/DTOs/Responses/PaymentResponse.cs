using PaymentService.Core.Enums;

namespace PaymentService.Application.DTOs.Responses;

public sealed class PaymentResponse
{
    public Guid PaymentId { get; set; }

    public Guid OrderId { get; set; }

    public PaymentStatus Status { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = string.Empty;

    public string? ExternalTransactionId { get; set; }

    public string? GatewayReference { get; set; }

    public string? RedirectUrl { get; set; }

    public string? Message { get; set; }
}
