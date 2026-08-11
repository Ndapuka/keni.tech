namespace PaymentService.Core.Events;

public sealed class PaymentRefundedDomainEvent : IDomainEvent
{
    public Guid PaymentId { get; }

    public DateTime OccurredOn { get; }

    public PaymentRefundedDomainEvent(Guid paymentId)
    {
        PaymentId = paymentId;
        OccurredOn = DateTime.UtcNow;
    }
}
