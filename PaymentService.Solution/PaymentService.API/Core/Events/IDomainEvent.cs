namespace PaymentService.Core.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}