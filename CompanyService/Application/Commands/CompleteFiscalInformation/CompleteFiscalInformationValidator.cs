using FluentValidation;

namespace CompanyService.Application.Commands.CompleteFiscalInformation;

public sealed class CompleteFiscalInformationValidator
    : AbstractValidator<CompleteFiscalInformationCommand>
{
    public CompleteFiscalInformationValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.TaxNumber)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Street)
            .NotEmpty()
            .MaximumLength(250);

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