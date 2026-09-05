using OrderService.DTOs;
using OrderService.Models;

namespace OrderService.Services;
public interface IOrdersService
{
    Task<List<Order>> GetAllOrdersAsync(Guid userId);
    Task<Order?> GetOrderByIdAsync(Guid orderId);

    Task<Order?> CreateOrderAsync(Guid userId);
    Task<Order?> CancelOrderAsync(Guid orderId);
}