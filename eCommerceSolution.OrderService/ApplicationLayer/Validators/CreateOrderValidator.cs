using ApplicationLayer.DTOs.Requests;
using FluentValidation;

namespace ApplicationLayer.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.OrderItems)
            .NotNull()
            .WithMessage("Order items are required.")
            .Must(x => x.Any())
            .WithMessage("The order must contain at least one item.");

        RuleForEach(x => x.OrderItems)
            .SetValidator(new CreateOrderItemValidator());

        RuleFor(x => x.ShippingAddress)
            .NotNull()
            .WithMessage("Shipping address is required.")
            .SetValidator(new ShippingAddressValidator());
    }
}