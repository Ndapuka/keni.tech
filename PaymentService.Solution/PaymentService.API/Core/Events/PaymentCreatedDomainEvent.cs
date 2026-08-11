namespace PaymentService.Core.Events;

public sealed class PaymentCreatedDomainEvent : IDomainEvent
{
    public Guid PaymentId { get; }

    public DateTime OccurredOn { get; }

    public PaymentCreatedDomainEvent(Guid paymentId)
    {
        PaymentId = paymentId;
        OccurredOn = DateTime.UtcNow;
    }
}