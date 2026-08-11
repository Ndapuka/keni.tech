using FluentValidation;
using Onboarding.Application.Commands;

namespace Onboarding.Application.Validators;

public class RegisterCompanyCommandValidator : AbstractValidator<RegisterCompanyCommand>
{
    public RegisterCompanyCommandValidator()
    {
        RuleFor(x => x.IdempotencyKey).NotEmpty();

        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PersonName).NotEmpty().MaximumLength(120);

        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.BusinessType).NotEmpty();
    }
}