namespace Onboarding.Contracts.Responses;

public class RegisterCompanyResponse
{
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public string Status { get; set; } = default!; // string, não o enum de Domain — Contracts não depende de Domain
}