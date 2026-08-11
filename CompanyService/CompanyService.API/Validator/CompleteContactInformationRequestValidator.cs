using CompanyService.API.Dtos.Requests.CompleteContactInformation;
using FluentValidation;

namespace CompanyService.API.Validators;

public sealed class CompleteContactInformationRequestValidator
    : AbstractValidator<CompleteContactInformationRequest>
{
    public CompleteContactInformationRequestValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(254)
            .EmailAddress();

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(30);
    }
}
