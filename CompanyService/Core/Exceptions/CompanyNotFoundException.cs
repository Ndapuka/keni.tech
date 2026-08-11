using BuildingBlocks.Shared.Contracts.Enums;

namespace CompanyService.Core.Exceptions;

public class CompanyNotFoundException : Exception
{
    public CompanyNotFoundException(Guid companyId)
        : base($"Company with Id '{companyId}' was not found.")
    {
    }

    public CompanyNotFoundException(string message)
        : base(message)
    {
    }
}

public sealed class CompanyMembershipRequiredException : Exception
{
    public Guid CompanyId { get; }
    public Guid UserId { get; }

    public CompanyMembershipRequiredException(Guid companyId, Guid userId)
        : base($"User {userId} is not an active member of company {companyId}.")
    {
        CompanyId = companyId;
        UserId = userId;
    }
}

public sealed class InsufficientCompanyRoleException : Exception
{
    public Guid CompanyId { get; }
    public Guid UserId { get; }
    public CompanyRole CurrentRole { get; }

    public InsufficientCompanyRoleException(Guid companyId, Guid userId, CompanyRole currentRole)
        : base($"User {userId} with role {currentRole} cannot manage company {companyId}.")
    {
        CompanyId = companyId;
        UserId = userId;
        CurrentRole = currentRole;
    }
}