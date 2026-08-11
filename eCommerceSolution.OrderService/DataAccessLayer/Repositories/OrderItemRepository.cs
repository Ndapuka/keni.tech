using BusinessLogicLayer.Entities;
using BusinessLogicLayer.RepositoryContracts;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly OrdersDbContext _context;

    public OrderItemRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<OrderItem> CreateAsync(OrderItem orderItem)
    {
        await _context.OrderItems.AddAsync(orderItem);
        return orderItem;
    }

    public async Task<OrderItem?> UpdateAsync(OrderItem orderItem)
    {
        var existingItem = await _context.OrderItems.FindAsync(orderItem.Id);

        if (existingItem == null)
            return null;

        _context.Entry(existingItem).CurrentValues.SetValues(orderItem);

        return existingItem;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _context.OrderItems.FindAsync(id);

        if (item == null)
            return false;

        _context.OrderItems.Remove(item);

        return true;
    }

    public async Task<OrderItem?> GetByIdAsync(Guid id)
    {
        return await _context.OrderItems.FindAsync(id);
    }

    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.OrderItems
            .Where(i => i.OrderId == orderId)
            .ToListAsync();
    }
}
