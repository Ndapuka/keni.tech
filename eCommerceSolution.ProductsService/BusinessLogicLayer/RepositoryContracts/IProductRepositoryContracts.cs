using BusinessLogicLayer.Entities;

namespace BusinessLogicLayer.RepositoryContracts;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);

    Task<IEnumerable<Product>> GetAllAsync();

    Task<IEnumerable<Product>> GetFeaturedProductsAsync();

    Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId);

    Task<IEnumerable<Product>> SearchProductsAsync(string keyword);

    Task AddAsync(Product product);

    void Update(Product product);

    void Delete(Product product);
}
