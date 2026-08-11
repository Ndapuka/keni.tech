using CompanyService.API.Dtos.Requests.UpdateCompany;
using FluentValidation;

namespace CompanyService.API.Validators;

public sealed class UpdateCompanyRequestValidator
    : AbstractValidator<UpdateCompanyRequest>
{
    public UpdateCompanyRequestValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.BusinessType)
            .IsInEnum();
    }
}