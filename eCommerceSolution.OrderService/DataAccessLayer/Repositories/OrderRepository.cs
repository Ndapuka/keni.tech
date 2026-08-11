using BusinessLogicLayer.Entities;
using BusinessLogicLayer.RepositoryContracts;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _context;

    public OrderRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        return order;
    }

    public async Task<Order?> UpdateAsync(Order order)
    {
        var existingOrder = await _context.Orders.FindAsync(order.Id);

        if (existingOrder == null)
            return null;

        _context.Entry(existingOrder).CurrentValues.SetValues(order);

        return existingOrder;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
            return false;

        _context.Orders.Remove(order);

        return true;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.ShippingAddress)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.ShippingAddress)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.ShippingAddress)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.ShippingAddress)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
    }
}