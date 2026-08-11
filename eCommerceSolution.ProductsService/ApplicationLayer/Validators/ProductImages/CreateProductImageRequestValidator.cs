using ApplicationLayer.DTOs.ProductImages;
using FluentValidation;

namespace ApplicationLayer.Validators.ProductImages;

public class CreateProductImageRequestValidator : AbstractValidator<CreateProductImageRequestDto>
{
    public CreateProductImageRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}
