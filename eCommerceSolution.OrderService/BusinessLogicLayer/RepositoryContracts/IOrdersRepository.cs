using BusinessLogicLayer.Entities;

namespace BusinessLogicLayer.RepositoryContracts;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);

    Task<Order?> UpdateAsync(Order order);

    Task<bool> DeleteAsync(Guid id);

    Task<Order?> GetByIdAsync(Guid id);

    Task<IEnumerable<Order>> GetAllAsync();

    Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);

    Task<Order?> GetByOrderNumberAsync(string orderNumber);
}