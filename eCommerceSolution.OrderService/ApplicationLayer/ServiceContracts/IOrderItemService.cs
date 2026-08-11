using ApplicationLayer.DTOs.Responses;

namespace ApplicationLayer.ServiceContracts;

public interface IOrderItemService
{
    Task<OrderItemResponse?> GetByIdAsync(Guid id);

    Task<IEnumerable<OrderItemResponse>> GetByOrderIdAsync(Guid orderId);

    Task<bool> DeleteAsync(Guid id);
}
