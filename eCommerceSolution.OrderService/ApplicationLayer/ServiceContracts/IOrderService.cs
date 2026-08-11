using ApplicationLayer.DTOs.Requests;
using ApplicationLayer.DTOs.Responses;

namespace ApplicationLayer.ServiceContracts;

public interface IOrderService
{
    Task<OrderResponse> CreateAsync(CreateOrderRequest request);

    Task<OrderResponse?> UpdateAsync(Guid id, UpdateOrderRequest request);

    Task<bool> DeleteAsync(Guid id);

    Task<OrderResponse?> GetByIdAsync(Guid id);

    Task<IEnumerable<OrderResponse>> GetAllAsync();

    Task<IEnumerable<OrderResponse>> GetByUserIdAsync(Guid userId);

    Task<OrderResponse?> GetByOrderNumberAsync(string orderNumber);
}