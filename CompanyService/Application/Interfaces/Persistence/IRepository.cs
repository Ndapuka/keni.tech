namespace CompanyService.Application.Interfaces.Persistence;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    void Update(TEntity entity);

    void Remove(TEntity entity);
}