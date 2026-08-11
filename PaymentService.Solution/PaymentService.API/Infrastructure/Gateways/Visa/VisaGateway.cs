using PaymentService.Application.DTOs.Requests;
using PaymentService.Application.DTOs.Responses;
using PaymentService.Application.ServiceContracts;
using PaymentService.Core.Enums;

namespace PaymentService.Infrastructure.Gateways.Visa;

public sealed class VisaGateway : IPaymentGateway
{
    public PaymentProvider Provider => throw new NotImplementedException();

    public async Task<PaymentResponse> ProcessPaymentAsync(
        CreatePaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        return new PaymentResponse
        {
            PaymentId = Guid.NewGuid(),
            OrderId = request.OrderId,
            Amount = request.Amount,
            Currency = request.Currency,
            Status = Core.Enums.PaymentStatus.Pending,
            Message = "Visa payment created successfully."
        };
    }

    public async Task<RefundResponse> RefundAsync(
        RefundPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        return new RefundResponse
        {
            PaymentId = request.PaymentId,
            Status = Core.Enums.PaymentStatus.Refunded,
            Message = "Refund completed successfully."
        };
    }
}