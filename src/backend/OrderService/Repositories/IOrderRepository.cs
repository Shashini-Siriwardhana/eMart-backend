using OrderService.Models;

namespace OrderService.Repositories;
public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync(Guid userId);

    Task<Order?> GetOrderByIdAsync(Guid orderId);

    Task<OrderItem?> GetItemByIdAsync(Guid orderId, Guid productId);
    Task AddOrderAsync(Order order);
    Task AddItemAsync(OrderItem item);

    void RemoveItemAsync(OrderItem item);

    Task SaveAsync();
}