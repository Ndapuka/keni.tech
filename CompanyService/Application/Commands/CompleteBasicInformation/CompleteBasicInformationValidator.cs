using FluentValidation;

namespace CompanyService.Application.Commands.CompleteBasicInformation;

public sealed class CompleteBasicInformationValidator
    : AbstractValidator<CompleteBasicInformationCommand>
{
    public CompleteBasicInformationValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .Matches("^[a-z0-9-]+$")
            .WithMessage("Slug must contain only lowercase letters, numbers and hyphens.");
    }
}
