using MediatR;
using PaymentService.Application.DTOs.Requests;
using PaymentService.Application.DTOs.Responses;

namespace PaymentService.Application.Features.Payments.Commands.RefundPayment;

public sealed record RefundPaymentCommand(
    RefundPaymentRequest Request)
    : IRequest<RefundResponse>;