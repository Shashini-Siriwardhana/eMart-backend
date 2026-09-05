namespace PaymentsService.Clients;

using System.Net;
using System.Net.Http.Json;
using PaymentsService.DTOs;

public class OrderApiClient : IOrderApiClient
{
    private readonly HttpClient _httpClient;
    public OrderApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OrderDto?> GetOrderAsync(Guid orderId)
    {
        var response = await _httpClient.GetAsync(
            $"api/order/{orderId}"
        );

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OrderDto>();
    }
}