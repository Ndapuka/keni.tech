using BusinessLogicLayer.Entities;

namespace BusinessLogicLayer.RepositoryContracts;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id);

    Task<IEnumerable<Category>> GetAllAsync();

    Task<Category?> GetByNameAsync(string name);

    Task AddAsync(Category category);

    void Update(Category category);

    void Delete(Category category);
}
