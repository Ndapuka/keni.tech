using CompanyService.API.Dtos.Requests.CompleteBranding;
using FluentValidation;

namespace CompanyService.API.Validators;

public sealed class CompleteBrandingRequestValidator
    : AbstractValidator<CompleteBrandingRequest>
{
    public CompleteBrandingRequestValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description is not null);

        RuleFor(x => x.LogoUrl)
            .MaximumLength(500)
            .Must(BeValidUrl)
            .When(x => !string.IsNullOrWhiteSpace(x.LogoUrl))
            .WithMessage("LogoUrl must be a valid URL.");
    }

    private static bool BeValidUrl(string? value)
    {
        return Uri.TryCreate(
            value,
            UriKind.Absolute,
            out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp
                || uri.Scheme == Uri.UriSchemeHttps);
    }
}
