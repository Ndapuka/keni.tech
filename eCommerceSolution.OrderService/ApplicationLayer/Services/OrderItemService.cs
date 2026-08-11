using ApplicationLayer.DTOs.Responses;
using ApplicationLayer.ServiceContracts;
using AutoMapper;
using BusinessLogicLayer.UnitOfWorkContracts;

namespace ApplicationLayer.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.OrderItems.DeleteAsync(id);

        if (!deleted)
            return false;

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<OrderItemResponse?> GetByIdAsync(Guid id)
    {
        var item = await _unitOfWork.OrderItems.GetByIdAsync(id);

        return item == null
            ? null
            : _mapper.Map<OrderItemResponse>(item);
    }

    public async Task<IEnumerable<OrderItemResponse>> GetByOrderIdAsync(Guid orderId)
    {
        var items = await _unitOfWork.OrderItems.GetByOrderIdAsync(orderId);

        return _mapper.Map<IEnumerable<OrderItemResponse>>(items);
    }
}
