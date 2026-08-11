using BuildingBlocks.Shared.Contracts.Enums;
using MediatR;
using Onboarding.Contracts.Responses;

namespace Onboarding.Application.Commands;

public class RegisterCompanyCommand : IRequest<RegisterCompanyResponse>
{
    public Guid IdempotencyKey { get; set; }

    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string PersonName { get; set; } = default!;
    public string? PhoneNumber { get; set; }

    public string CompanyName { get; set; } = default!;
    public BusinessType BusinessType { get; set; } = default!;
    public string? Country { get; set; }
    public string? City { get; set; }
}