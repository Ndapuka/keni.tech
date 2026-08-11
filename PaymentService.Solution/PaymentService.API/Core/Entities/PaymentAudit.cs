using PaymentService.Core.Common;

namespace PaymentService.Core.Entities;

public sealed class PaymentAudit : AuditableEntity
{
    public Guid PaymentId { get; private set; }

    public string Action { get; private set; } = string.Empty;

    public string? OldStatus { get; private set; }

    public string? NewStatus { get; private set; }

    public string? PerformedBy { get; private set; }

    public string? IpAddress { get; private set; }

    public string? UserAgent { get; private set; }

    public string? CorrelationId { get; private set; }

    protected PaymentAudit()
    {
    }

    public PaymentAudit(
        Guid paymentId,
        string action,
        string? oldStatus,
        string? newStatus,
        string? performedBy,
        string? ipAddress,
        string? userAgent,
        string? correlationId)
    {
        PaymentId = paymentId;
        Action = action;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        PerformedBy = performedBy;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        CorrelationId = correlationId;
    }
}