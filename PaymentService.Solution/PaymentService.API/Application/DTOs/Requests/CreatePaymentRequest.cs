using PaymentService.Core.Enums;

namespace PaymentService.Application.DTOs.Requests;

public sealed class CreatePaymentRequest
{
    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "EUR";

    public PaymentMethod PaymentMethod { get; set; }

    public PaymentProvider Provider { get; set; }

    public string? Description { get; set; }
}
