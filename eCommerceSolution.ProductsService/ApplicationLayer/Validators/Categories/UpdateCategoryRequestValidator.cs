using ApplicationLayer.DTOs.Categories;
using FluentValidation;

namespace ApplicationLayer.Validators.Categories;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequestDto>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}