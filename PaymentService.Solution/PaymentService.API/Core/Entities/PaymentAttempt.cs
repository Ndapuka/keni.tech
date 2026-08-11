using PaymentService.Core.Common;
using PaymentService.Core.Enums;

namespace PaymentService.Core.Entities;

public sealed class PaymentAttempt : AuditableEntity
{
    public Guid PaymentId { get; private set; }

    public PaymentProvider Provider { get; private set; }

    public int AttemptNumber { get; private set; }

    public PaymentStatus Status { get; private set; }

    public string? ResponseCode { get; private set; }

    public string? ResponseMessage { get; private set; }

    public long DurationMilliseconds { get; private set; }

    protected PaymentAttempt()
    {
    }

    public PaymentAttempt(
        Guid paymentId,
        PaymentProvider provider,
        int attemptNumber,
        PaymentStatus status,
        string? responseCode,
        string? responseMessage,
        long durationMilliseconds)
    {
        PaymentId = paymentId;
        Provider = provider;
        AttemptNumber = attemptNumber;
        Status = status;
        ResponseCode = responseCode;
        ResponseMessage = responseMessage;
        DurationMilliseconds = durationMilliseconds;
    }
}