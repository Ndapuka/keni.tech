using BuildingBlocks.Shared.Contracts.Enums;

namespace Onboarding.Contracts.Requests;

public class RegisterCompanyRequest
{
    // Idempotência: o frontend gera este GUID uma vez por tentativa de submissão do form.
    // Reenvios com a mesma key não devem criar user/company duplicados.
    public Guid IdempotencyKey { get; set; }

    // Dados do User
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string PersonName { get; set; } = default!;
    public string? PhoneNumber { get; set; }

    // Dados da Company
    public string CompanyName { get; set; } = default!;
    public BusinessType BusinessType { get; set; } = default!;
    public string? Country { get; set; }
    public string? City { get; set; }
}