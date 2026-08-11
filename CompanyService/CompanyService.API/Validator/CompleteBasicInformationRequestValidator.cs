using CompanyService.API.Dtos.Requests.CompleteBasicInformation;
using FluentValidation;

namespace CompanyService.API.Validators;

public sealed class CompleteBasicInformationRequestValidator
    : AbstractValidator<CompleteBasicInformationRequest>
{
    public CompleteBasicInformationRequestValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(150)
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage(
                "Slug must contain only lowercase letters, numbers and hyphens.");
    }
}
