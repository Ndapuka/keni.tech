using PaymentService.Application.DTOs.Requests;
using PaymentService.Application.DTOs.Responses;
using PaymentService.Application.ServiceContracts;
using PaymentService.Core.Enums;
using PaymentService.Infrastructure.Gateways.MbWay;
using PaymentService.Infrastructure.Gateways.Visa;

namespace PaymentService.Infrastructure.Services;

public sealed class PaymentGatewayService : IPaymentGatewayService
{
    private readonly IEnumerable<IPaymentGateway> _gateways;

    public PaymentGatewayService(IEnumerable<IPaymentGateway> gateways)
    {
        _gateways = gateways;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(
        CreatePaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var gateway = _gateways.FirstOrDefault(g => g.Provider == request.Provider);

        if (gateway is null)
            throw new NotSupportedException($"Provider {request.Provider} is not supported.");

        return await gateway.ProcessPaymentAsync(request, cancellationToken);
    }

    public async Task<RefundResponse> RefundAsync(
        RefundPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        // Quando tiveres persistência, aqui poderás selecionar o gateway
        // com base no Payment armazenado.
        throw new NotImplementedException();
    }
}