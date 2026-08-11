using ApplicationLayer.DTOs.Requests;
using ApplicationLayer.DTOs.Responses;
using ApplicationLayer.ServiceContracts;
using AutoMapper;
using BusinessLogicLayer.UnitOfWorkContracts;

namespace ApplicationLayer.Services;

public class ShippingAddressService : IShippingAddressService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ShippingAddressService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.ShippingAddresses.DeleteAsync(id);

        if (!deleted)
            return false;

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<ShippingAddressResponse?> GetByIdAsync(Guid id)
    {
        var address = await _unitOfWork.ShippingAddresses.GetByIdAsync(id);

        return address == null
            ? null
            : _mapper.Map<ShippingAddressResponse>(address);
    }

    public async Task<ShippingAddressResponse?> GetByOrderIdAsync(Guid orderId)
    {
        var address = await _unitOfWork.ShippingAddresses.GetByOrderIdAsync(orderId);

        return address == null
            ? null
            : _mapper.Map<ShippingAddressResponse>(address);
    }

    public async Task<ShippingAddressResponse?> UpdateAsync(Guid id, ShippingAddressRequest request)
    {
        var address = await _unitOfWork.ShippingAddresses.GetByIdAsync(id);

        if (address == null)
            return null;

        _mapper.Map(request, address);

        await _unitOfWork.ShippingAddresses.UpdateAsync(address);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ShippingAddressResponse>(address);
    }
}