namespace PaymentService.Core.Events;

public sealed class PaymentCancelledDomainEvent : IDomainEvent
{
    public Guid PaymentId { get; }

    public DateTime OccurredOn { get; }

    public PaymentCancelledDomainEvent(Guid paymentId)
    {
        PaymentId = paymentId;
        OccurredOn = DateTime.UtcNow;
    }
}
