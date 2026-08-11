namespace smartRestaurant.Core.UnitOfWorkContrats;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();

}
