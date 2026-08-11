namespace PaymentService.Application.DTOs.Requests;

public sealed class RefundPaymentRequest
{
    public Guid PaymentId { get; set; }

    public string? Reason { get; set; }
}