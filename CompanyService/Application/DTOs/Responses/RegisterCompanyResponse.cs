namespace CompanyService.Application.DTOs.Responses;

public sealed class RegisterCompanyResponse
{
    public Guid CompanyId { get; init; }

    public string Status { get; init; } = string.Empty;

    public string WizardStep { get; init; } = string.Empty;
}
