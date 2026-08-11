using ApplicationLayer.DTOs.External;

namespace ApplicationLayer.HttpClientsContracts;

public interface IProductsServiceClient
{
    Task<ProductResponse?> GetProductByIdAsync(Guid productId);
}

