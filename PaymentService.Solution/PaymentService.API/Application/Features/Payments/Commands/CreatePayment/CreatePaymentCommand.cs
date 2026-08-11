using MediatR;
using PaymentService.Application.DTOs.Requests;
using PaymentService.Application.DTOs.Responses;

namespace PaymentService.Application.Features.Payments.Commands.CreatePayment;

public sealed record CreatePaymentCommand(
    CreatePaymentRequest Request)
    : IRequest<PaymentResponse>;