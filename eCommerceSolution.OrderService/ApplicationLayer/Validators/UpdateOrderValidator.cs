using ApplicationLayer.DTOs.Requests;
using FluentValidation;

namespace ApplicationLayer.Validators;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid order status.");

        RuleFor(x => x.PaymentStatus)
            .IsInEnum()
            .WithMessage("Invalid payment status.");
    }
}
