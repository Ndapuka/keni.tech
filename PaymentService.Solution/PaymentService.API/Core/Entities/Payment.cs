using PaymentService.Core.Common;
using PaymentService.Core.Enums;
using PaymentService.Core.Events;
using PaymentService.Core.ValueObjects;

namespace PaymentService.Core.Entities;

public sealed class Payment : AggregateRoot
{
    public Guid OrderId { get; private set; }

    public Guid UserId { get; private set; }

    public Money Amount { get; private set; } = null!;

    public PaymentMethod PaymentMethod { get; private set; }

    public PaymentProvider Provider { get; private set; }

    public PaymentStatus Status { get; private set; }

    public string? ExternalTransactionId { get; private set; }

    public string? GatewayReference { get; private set; }

    public string? Description { get; private set; }

    protected Payment()
    {
    }

    public Payment(
        Guid orderId,
        Guid userId,
        Money amount,
        PaymentMethod paymentMethod,
        PaymentProvider provider,
        string? description = null)
    {
        OrderId = orderId;
        UserId = userId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Provider = provider;
        Description = description;

        Status = PaymentStatus.Pending;

        AddDomainEvent(new PaymentCreatedDomainEvent(Id));
    }

    public void MarkAsProcessing()
    {
        Status = PaymentStatus.Processing;
        MarkAsUpdated();
    }

    public void MarkAsPaid(string transactionId, string? gatewayReference = null)
    {
        Status = PaymentStatus.Paid;
        ExternalTransactionId = transactionId;
        GatewayReference = gatewayReference;

        MarkAsUpdated();

        AddDomainEvent(new PaymentSucceededDomainEvent(Id)); // erro aqui
    }

    public void MarkAsFailed()
    {
        Status = PaymentStatus.Failed;

        MarkAsUpdated();

        AddDomainEvent(new PaymentFailedDomainEvent(Id));
    }

    public void Cancel()
    {
        Status = PaymentStatus.Cancelled;

        MarkAsUpdated();

        AddDomainEvent(new PaymentCancelledDomainEvent(Id));
    }

    public void Refund()
    {
        Status = PaymentStatus.Refunded;

        MarkAsUpdated();

        AddDomainEvent(new PaymentRefundedDomainEvent(Id));
    }
}