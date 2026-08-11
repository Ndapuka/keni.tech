namespace PaymentService.Core.Events;

public sealed class PaymentSucceededDomainEvent : IDomainEvent
{
    public Guid PaymentId { get; }

    public DateTime OccurredOn { get; }

    public PaymentSucceededDomainEvent(Guid paymentId)
    {
        PaymentId = paymentId;
        OccurredOn = DateTime.UtcNow;
    }
}