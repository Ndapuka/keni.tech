namespace CompanyService.Core.Events;

public sealed record CompanyRegisteredEvent(
    Guid CompanyId,
    Guid OwnerUserId);
