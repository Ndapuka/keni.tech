
using smartRestaurant.Application.DTO;
//using smartRestaurant.Core.DTO;
using FluentValidation;

namespace smartRestaurant.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.PersonName)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(3, 50).WithMessage("O nome deve ter entre 3 e 50 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("O email deve conter um domínio válido."); ;

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A password é obrigatória.")
            .Length(6, 50).WithMessage("A password deve ter entre 6 e 50 caracteres.");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Género inválido.");
    }
}