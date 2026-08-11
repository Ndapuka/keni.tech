using BusinessLogicLayer.Entities;

namespace BusinessLogicLayer.RepositoryContracts;

public interface IOrderItemRepository
{
    Task<OrderItem> CreateAsync(OrderItem orderItem);

    Task<OrderItem?> UpdateAsync(OrderItem orderItem);

    Task<bool> DeleteAsync(Guid id);

    Task<OrderItem?> GetByIdAsync(Guid id);

    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId);
}