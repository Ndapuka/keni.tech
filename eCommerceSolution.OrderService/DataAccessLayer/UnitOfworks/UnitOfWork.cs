using BusinessLogicLayer.RepositoryContracts;
using BusinessLogicLayer.UnitOfWorkContracts;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;

namespace DataAccessLayer.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly OrdersDbContext _context;

    public IOrderRepository Orders { get; }

    public IOrderItemRepository OrderItems { get; }

    public IShippingAddressRepository ShippingAddresses { get; }

    public UnitOfWork(OrdersDbContext context)
    {
        _context = context;

        Orders = new OrderRepository(_context);
        OrderItems = new OrderItemRepository(_context);
        ShippingAddresses = new ShippingAddressRepository(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
