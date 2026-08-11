using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.API.Dtos.Requests.InviteUser;
using FluentValidation;

namespace CompanyService.API.Validators;

public sealed class InviteUserRequestValidator
    : AbstractValidator<InviteUserRequest>
{
    public InviteUserRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Role)
            .IsInEnum()
            .Must(role => role != CompanyRole.Owner)
            .WithMessage("The Owner role cannot be assigned through an invitation.");
    }
}