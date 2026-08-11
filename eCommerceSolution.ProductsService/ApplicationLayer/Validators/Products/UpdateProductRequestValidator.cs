using ApplicationLayer.DTOs.Products;
using FluentValidation;

namespace ApplicationLayer.Validators.Products;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequestDto>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.PromotionalPrice)
            .GreaterThan(0)
            .When(x => x.PromotionalPrice.HasValue);

        RuleFor(x => x.PreparationTimeHours)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MinimumAdvanceHours)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MaximumDailyQuantity)
            .GreaterThan(0);
    }
}
