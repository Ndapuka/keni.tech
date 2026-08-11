namespace PaymentService.Application.DTOs.Requests;

public sealed class CancelPaymentRequest
{
    public Guid PaymentId { get; set; }

    public string? Reason { get; set; }
}
