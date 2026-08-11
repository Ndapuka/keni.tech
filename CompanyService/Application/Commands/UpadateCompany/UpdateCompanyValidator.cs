using FluentValidation;

namespace CompanyService.Application.Commands.UpdateCompany;

public sealed class UpdateCompanyValidator
    : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.BusinessType)
            .IsInEnum();
    }
}