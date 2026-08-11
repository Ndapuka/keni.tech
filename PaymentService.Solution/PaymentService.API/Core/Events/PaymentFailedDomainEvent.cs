namespace PaymentService.Core.Events;

public sealed class PaymentFailedDomainEvent : IDomainEvent
{
    public Guid PaymentId { get; }

    public DateTime OccurredOn { get; }

    public PaymentFailedDomainEvent(Guid paymentId)
    {
        PaymentId = paymentId;
        OccurredOn = DateTime.UtcNow;
    }
}