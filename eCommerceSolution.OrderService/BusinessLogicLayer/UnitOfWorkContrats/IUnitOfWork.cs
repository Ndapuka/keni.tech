using BusinessLogicLayer.RepositoryContracts;

namespace BusinessLogicLayer.UnitOfWorkContracts;

public interface IUnitOfWork
{
    IOrderRepository Orders { get; }

    IOrderItemRepository OrderItems { get; }

    IShippingAddressRepository ShippingAddresses { get; }

    Task<int> SaveChangesAsync();
}