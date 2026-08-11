using MediatR;
using PaymentService.Application.DTOs.Requests;

namespace PaymentService.Application.Features.Payments.Commands.CancelPayment;

public sealed record CancelPaymentCommand(
    CancelPaymentRequest Request)
    : IRequest;