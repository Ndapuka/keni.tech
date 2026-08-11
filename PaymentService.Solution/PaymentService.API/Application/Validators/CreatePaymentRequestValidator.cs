using FluentValidation;
using PaymentService.Application.DTOs.Requests;

namespace PaymentService.Application.Validators;

public sealed class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentRequestValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .MaximumLength(3);

        RuleFor(x => x.PaymentMethod)
            .IsInEnum();

        RuleFor(x => x.Provider)
            .IsInEnum();

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}