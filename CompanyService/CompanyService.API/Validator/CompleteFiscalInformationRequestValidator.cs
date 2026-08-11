using CompanyService.API.Dtos.Requests.CompleteFiscalInformation;
using FluentValidation;

namespace CompanyService.API.Validators;

public sealed class CompleteFiscalInformationRequestValidator
    : AbstractValidator<CompleteFiscalInformationRequest>
{
    public CompleteFiscalInformationRequestValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.TaxNumber)
            .NotEmpty()
            .MaximumLength(50);

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