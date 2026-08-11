

using smartRestaurant.Application.DTO;
using FluentValidation;

namespace smartRestaurant.Application.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.PersonName)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MinimumLength(3);

        RuleFor(x => x.Gender)
            .IsInEnum();

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20);
    }
}
