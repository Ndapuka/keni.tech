using PaymentService.Core.Enums;

namespace PaymentService.Application.DTOs.Responses;

public sealed class RefundResponse
{
    public Guid PaymentId { get; set; }

    public PaymentStatus Status { get; set; }

    public string? Message { get; set; }
}