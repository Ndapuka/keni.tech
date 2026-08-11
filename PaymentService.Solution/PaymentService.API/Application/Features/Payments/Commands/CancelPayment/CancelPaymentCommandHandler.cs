using MediatR;
using PaymentService.Application.ServiceContracts;
using PaymentService.Core.Interfaces.Repositories;

namespace PaymentService.Application.Features.Payments.Commands.CancelPayment;

public sealed class CancelPaymentCommandHandler
    : IRequestHandler<CancelPaymentCommand>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IAuditService _auditService;
    private readonly IUnitOfWork _unitOfWork;

    public CancelPaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IAuditService auditService,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _auditService = auditService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        CancelPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(
            request.Request.PaymentId,
            cancellationToken);

        if (payment is null)
            throw new KeyNotFoundException("Payment not found.");

        payment.Cancel();

        _paymentRepository.Update(payment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}