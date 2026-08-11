using ApplicationLayer.DTOs.Requests;
using FluentValidation;

namespace ApplicationLayer.Validators;

public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemRequest>
{
    public CreateOrderItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");

        RuleFor(x => x.ProductName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}