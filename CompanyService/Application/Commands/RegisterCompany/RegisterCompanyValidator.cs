using FluentValidation;

namespace CompanyService.Application.Commands.RegisterCompany;

public sealed class RegisterCompanyValidator
    : AbstractValidator<RegisterCompanyCommand>
{
    public RegisterCompanyValidator()
    {
        RuleFor(x => x.OwnerUserId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

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
