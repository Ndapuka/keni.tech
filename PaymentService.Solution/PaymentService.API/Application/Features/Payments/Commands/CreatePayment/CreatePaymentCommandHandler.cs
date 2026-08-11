using AutoMapper;
using MediatR;
using PaymentService.Application.DTOs.Responses;
using PaymentService.Application.ServiceContracts;
using PaymentService.Core.Interfaces.Repositories;

namespace PaymentService.Application.Features.Payments.Commands.CreatePayment;

public sealed class CreatePaymentCommandHandler
    : IRequestHandler<CreatePaymentCommand, PaymentResponse>
{
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IAuditService _auditService;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentCommandHandler(
        IPaymentGatewayService paymentGatewayService,
        IAuditService auditService,
        IUnitOfWork unitOfWork)
    {
        _paymentGatewayService = paymentGatewayService;
        _auditService = auditService;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaymentResponse> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var response = await _paymentGatewayService.ProcessPaymentAsync(
            request.Request,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return response;
    }
}