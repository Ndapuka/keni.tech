using BusinessLogicLayer.Entities;
using BusinessLogicLayer.RepositoryContracts;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ShippingAddressRepository : IShippingAddressRepository
{
    private readonly OrdersDbContext _context;

    public ShippingAddressRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<ShippingAddress> CreateAsync(ShippingAddress shippingAddress)
    {
        await _context.ShippingAddresses.AddAsync(shippingAddress);
        return shippingAddress;
    }

    public async Task<ShippingAddress?> UpdateAsync(ShippingAddress shippingAddress)
    {
        var existingAddress = await _context.ShippingAddresses.FindAsync(shippingAddress.Id);

        if (existingAddress == null)
            return null;

        _context.Entry(existingAddress).CurrentValues.SetValues(shippingAddress);

        return existingAddress;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var address = await _context.ShippingAddresses.FindAsync(id);

        if (address == null)
            return false;

        _context.ShippingAddresses.Remove(address);

        return true;
    }

    public async Task<ShippingAddress?> GetByIdAsync(Guid id)
    {
        return await _context.ShippingAddresses.FindAsync(id);
    }

    public async Task<ShippingAddress?> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.ShippingAddresses
            .FirstOrDefaultAsync(a => a.OrderId == orderId);
    }
}