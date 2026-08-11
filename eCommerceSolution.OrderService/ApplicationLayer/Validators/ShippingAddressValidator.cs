using ApplicationLayer.DTOs.Requests;
using FluentValidation;

namespace ApplicationLayer.Validators;

public class ShippingAddressValidator : AbstractValidator<ShippingAddressRequest>
{
    public ShippingAddressValidator()
    {
        RuleFor(x => x.RecipientName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Street)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);
    }
}
