using System.Net.Http.Json;
using ApplicationLayer.DTOs.External;
using ApplicationLayer.HttpClientsContracts;

namespace ApplicationLayer.HttpClients;

public class ProductsServiceClient : IProductsServiceClient
{
    private readonly HttpClient _httpClient;

    public ProductsServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProductResponse?> GetProductByIdAsync(Guid productId)
    {
        return await _httpClient.GetFromJsonAsync<ProductResponse>(
            $"api/products/{productId}");
    }
}