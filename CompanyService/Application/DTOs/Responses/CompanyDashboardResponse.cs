using BuildingBlocks.Shared.Contracts.Enums;

namespace CompanyService.Application.DTOs.Responses;

public sealed class CompanyDashboardResponse
{
    public Guid CompanyId { get; init; }

    public string CompanyName { get; init; } = string.Empty;

    public CompanyStatus Status { get; init; }

    public CompanyWizardStep WizardStep { get; init; }

    public bool WizardCompleted =>
        WizardStep == CompanyWizardStep.Completed;
}
