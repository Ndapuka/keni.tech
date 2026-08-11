namespace CompanyService.Core.Events;

public sealed record WizardCompletedEvent(
    Guid CompanyId);