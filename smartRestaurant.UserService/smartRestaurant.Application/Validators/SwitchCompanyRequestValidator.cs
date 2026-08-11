using FluentValidation;
using smartRestaurant.Application.DTO;
namespace smartRestaurant.Application.Validators;

public class SwitchCompanyRequestValidator
    : AbstractValidator<SwitchCompanyRequest>
{
    public SwitchCompanyRequestValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("CompanyId is required.");
    }
}
