using FluentValidation;
using PaymentService.Application.DTOs.Requests;

namespace PaymentService.Application.Validators;

public sealed class RefundPaymentRequestValidator : AbstractValidator<RefundPaymentRequest>
{
    public RefundPaymentRequestValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty();

        RuleFor(x => x.Reason)
            .MaximumLength(500);
    }
}
