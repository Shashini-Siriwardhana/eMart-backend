using PaymentsService.DTOs;

namespace PaymentsService.Clients;

public interface IOrderApiClient
{
    Task<OrderDto?> GetOrderAsync(Guid orderId);
}