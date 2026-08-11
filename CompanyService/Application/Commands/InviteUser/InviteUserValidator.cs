using BuildingBlocks.Shared.Contracts.Company.Common;
using BuildingBlocks.Shared.Contracts.Enums;
using FluentValidation;

namespace CompanyService.Application.Commands.InviteUser;

public sealed class InviteUserValidator
    : AbstractValidator<InviteUserCommand>
{
    public InviteUserValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Role)
            .IsInEnum();
    }
}