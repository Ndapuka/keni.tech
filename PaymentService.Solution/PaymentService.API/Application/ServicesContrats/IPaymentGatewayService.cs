using PaymentService.Application.DTOs.Requests;
using PaymentService.Application.DTOs.Responses;

namespace PaymentService.Application.ServiceContracts;

public interface IPaymentGatewayService
{
    Task<PaymentResponse> ProcessPaymentAsync(
        CreatePaymentRequest request,
        CancellationToken cancellationToken = default);

    Task<RefundResponse> RefundAsync(
        RefundPaymentRequest request,
        CancellationToken cancellationToken = default);
}