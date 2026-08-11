using MediatR;
using PaymentService.Application.DTOs.Responses;

namespace PaymentService.Application.Features.Payments.Queries.GetPaymentById;

public sealed record GetPaymentByIdQuery(Guid PaymentId)
    : IRequest<PaymentStatusResponse?>;
