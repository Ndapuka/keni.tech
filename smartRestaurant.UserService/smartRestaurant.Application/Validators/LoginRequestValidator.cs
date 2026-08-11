
using smartRestaurant.Application.DTO;
using smartRestaurant.Core.DTO;
using FluentValidation;


namespace smartRestaurant.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A password é obrigatória.");
    }
}

