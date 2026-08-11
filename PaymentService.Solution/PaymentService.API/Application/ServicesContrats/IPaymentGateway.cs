using PaymentService.Application.DTOs.Requests;
using PaymentService.Application.DTOs.Responses;
using PaymentService.Core.Enums;

namespace PaymentService.Application.ServiceContracts;

public interface IPaymentGateway
{
    PaymentProvider Provider { get; }

    Task<PaymentResponse> ProcessPaymentAsync(
        CreatePaymentRequest request,
        CancellationToken cancellationToken = default);

    Task<RefundResponse> RefundAsync(
        RefundPaymentRequest request,
        CancellationToken cancellationToken = default);
}