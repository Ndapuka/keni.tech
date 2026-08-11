using AutoMapper;
using MediatR;
using PaymentService.Application.DTOs.Responses;
using PaymentService.Core.Interfaces.Repositories;

namespace PaymentService.Application.Features.Payments.Queries.GetPaymentById;

public sealed class GetPaymentByIdQueryHandler
    : IRequestHandler<GetPaymentByIdQuery, PaymentStatusResponse?>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public GetPaymentByIdQueryHandler(
        IPaymentRepository paymentRepository,
        IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<PaymentStatusResponse?> Handle(
        GetPaymentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(
            request.PaymentId,
            cancellationToken);

        if (payment is null)
            return null;

        return _mapper.Map<PaymentStatusResponse>(payment);
    }
}