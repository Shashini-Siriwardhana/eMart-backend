using CartsService.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace CartsService.Clients;

public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _httpClient;

    public ProductApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<ProductDto?> GetProductByIdAsync(Guid productId)
    {
        var response = await _httpClient.GetAsync(
            $"api/products/{productId}"
        );

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ProductDto>();
    }
}