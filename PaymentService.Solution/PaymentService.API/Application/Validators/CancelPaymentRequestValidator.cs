using FluentValidation;
using PaymentService.Application.DTOs.Requests;

namespace PaymentService.Application.Validators;

public sealed class CancelPaymentRequestValidator : AbstractValidator<CancelPaymentRequest>
{
    public CancelPaymentRequestValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty();

        RuleFor(x => x.Reason)
            .MaximumLength(500);
    }
}
