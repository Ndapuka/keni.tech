using CompanyService.API.Dtos.Requests.RegisterCompany;
using FluentValidation;

namespace CompanyService.API.Validators;

public sealed class RegisterCompanyRequestValidator
    : AbstractValidator<RegisterCompanyRequest>
{
    public RegisterCompanyRequestValidator()
    {
        RuleFor(x => x.OwnerUserId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.BusinessType)
            .IsInEnum();

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);
    }
}