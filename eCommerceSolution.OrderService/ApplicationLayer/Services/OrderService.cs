using ApplicationLayer.DTOs.Requests;
using ApplicationLayer.DTOs.Responses;
using ApplicationLayer.ServiceContracts;
using AutoMapper;
using BusinessLogicLayer.Entities;
using BusinessLogicLayer.UnitOfWorkContracts;

namespace ApplicationLayer.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
    {
        var order = _mapper.Map<Order>(request);

        order.Id = Guid.NewGuid();
        order.OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}";
        order.OrderDate = DateTime.UtcNow;

        order.SubTotal = order.OrderItems.Sum(x => x.UnitPrice * x.Quantity);
        order.TotalAmount = order.SubTotal - order.Discount + order.ShippingCost;

        foreach (var item in order.OrderItems)
        {
            item.Id = Guid.NewGuid();
            item.OrderId = order.Id;
            item.TotalPrice = item.UnitPrice * item.Quantity;
        }

        if (order.ShippingAddress != null)
        {
            order.ShippingAddress.Id = Guid.NewGuid();
            order.ShippingAddress.OrderId = order.Id;
        }

        await _unitOfWork.Orders.CreateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<OrderResponse?> UpdateAsync(Guid id, UpdateOrderRequest request)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);

        if (order == null)
            return null;

        order.Status = request.Status;
        order.PaymentStatus = request.PaymentStatus;
        order.Notes = request.Notes;

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Orders.DeleteAsync(id);

        if (!deleted)
            return false;

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<OrderResponse>> GetAllAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();

        return _mapper.Map<IEnumerable<OrderResponse>>(orders);
    }

    public async Task<OrderResponse?> GetByIdAsync(Guid id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);

        return order == null
            ? null
            : _mapper.Map<OrderResponse>(order);
    }

    public async Task<IEnumerable<OrderResponse>> GetByUserIdAsync(Guid userId)
    {
        var orders = await _unitOfWork.Orders.GetByUserIdAsync(userId);

        return _mapper.Map<IEnumerable<OrderResponse>>(orders);
    }

    public async Task<OrderResponse?> GetByOrderNumberAsync(string orderNumber)
    {
        var order = await _unitOfWork.Orders.GetByOrderNumberAsync(orderNumber);

        return order == null
            ? null
            : _mapper.Map<OrderResponse>(order);
    }
}