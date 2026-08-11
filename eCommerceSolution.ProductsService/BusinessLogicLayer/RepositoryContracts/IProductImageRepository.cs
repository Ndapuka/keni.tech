using BusinessLogicLayer.Entities;

namespace BusinessLogicLayer.RepositoryContracts;

public interface IProductImageRepository
{
    Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(Guid productId);

    Task AddAsync(ProductImage image);

    void Delete(ProductImage image);
    Task<ProductImage?> GetByIdAsync(Guid imageId);
}