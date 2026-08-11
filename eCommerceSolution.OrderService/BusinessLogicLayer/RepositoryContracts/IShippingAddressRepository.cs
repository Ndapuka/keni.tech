using BusinessLogicLayer.Entities;

namespace BusinessLogicLayer.RepositoryContracts;

public interface IShippingAddressRepository
{
    Task<ShippingAddress> CreateAsync(ShippingAddress shippingAddress);

    Task<ShippingAddress?> UpdateAsync(ShippingAddress shippingAddress);

    Task<bool> DeleteAsync(Guid id);

    Task<ShippingAddress?> GetByIdAsync(Guid id);

    Task<ShippingAddress?> GetByOrderIdAsync(Guid orderId);
}