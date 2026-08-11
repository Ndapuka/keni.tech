using FluentValidation;

namespace CompanyService.Application.Commands.CompleteContactInformation;

public sealed class CompleteContactInformationValidator
    : AbstractValidator<CompleteContactInformationCommand>
{
    public CompleteContactInformationValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(20);
    }
}