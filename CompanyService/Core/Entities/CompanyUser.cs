using BuildingBlocks.Shared.Contracts.Enums;

namespace CompanyService.Core.Entities;

public sealed class CompanyUser
{
    protected CompanyUser()
    {
    }

    public CompanyUser(
        Guid userId,
        CompanyRole role)
    {
        Id = Guid.NewGuid();

        UserId = userId;

        Role = role;

        JoinedAt = DateTime.UtcNow;

        IsActive = true;
    }

    public Guid Id { get; private set; }

    // FK utilizada pelo EF Core
    public Guid CompanyId { get; private set; }

    /// <summary>
    /// Referência ao utilizador existente no UserService.
    /// </summary>
    public Guid UserId { get; private set; }

    public CompanyRole Role { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime JoinedAt { get; private set; }

    public void ChangeRole(CompanyRole role)
    {
        Role = role;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}