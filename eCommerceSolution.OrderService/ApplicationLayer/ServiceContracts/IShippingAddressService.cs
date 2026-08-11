using ApplicationLayer.DTOs.Requests;
using ApplicationLayer.DTOs.Responses;

namespace ApplicationLayer.ServiceContracts;

public interface IShippingAddressService
{
    Task<ShippingAddressResponse?> GetByIdAsync(Guid id);

    Task<ShippingAddressResponse?> GetByOrderIdAsync(Guid orderId);

    Task<ShippingAddressResponse?> UpdateAsync(Guid id, ShippingAddressRequest request);

    Task<bool> DeleteAsync(Guid id);
}