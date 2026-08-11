using FluentValidation;

namespace CompanyService.Application.Commands.CompleteBranding;

public sealed class CompleteBrandingValidator
    : AbstractValidator<CompleteBrandingCommand>
{
    public CompleteBrandingValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.LogoUrl)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.LogoUrl));
    }
}
