namespace CompanyService.Core.UserReference;

public sealed class CompanyOwner
{
    public Guid UserId { get; }

    public CompanyOwner(Guid userId)
    {
        UserId = userId;
    }
}