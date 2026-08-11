namespace BuildingBlocks.Shared.Contracts.Company.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; protected set; }

    public Guid? CreatedBy { get; protected set; }

    public Guid? UpdatedBy { get; protected set; }

    public void SetCreatedBy(Guid userId)
    {
        CreatedBy = userId;
    }



    public void SetUpdatedBy(Guid userId)
    {
        UpdatedBy = userId;
    }
}