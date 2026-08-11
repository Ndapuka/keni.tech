namespace CompanyService.Core.Events;

public sealed record CompanyActivatedEvent(
    Guid CompanyId);