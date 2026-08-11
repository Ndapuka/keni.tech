using MediatR;
using PaymentService.Application.DTOs.Responses;
using PaymentService.Application.ServiceContracts;
using PaymentService.Core.Interfaces.Repositories;

namespace PaymentService.Application.Features.Payments.Commands.RefundPayment;

public sealed class RefundPaymentCommandHandler
    : IRequestHandler<RefundPaymentCommand, RefundResponse>
{
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IAuditService _auditService;
    private readonly IUnitOfWork _unitOfWork;

    public RefundPaymentCommandHandler(
        IPaymentGatewayService paymentGatewayService,
        IAuditService auditService,
        IUnitOfWork unitOfWork)
    {
        _paymentGatewayService = paymentGatewayService;
        _auditService = auditService;
        _unitOfWork = unitOfWork;
    }

    public async Task<RefundResponse> Handle(
        RefundPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var response = await _paymentGatewayService.RefundAsync(
            request.Request,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return response;
    }
}