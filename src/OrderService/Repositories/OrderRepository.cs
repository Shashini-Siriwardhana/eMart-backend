using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync(Guid userId)
    {
        return await _context.Orders.Where(order => order.UserId == userId)
        .Include(order => order.OrderItems)
        .ToListAsync();
    } 

    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        return await _context.Orders.Where(order => order.Id == orderId)
        .Include(order => order.OrderItems)
        .FirstOrDefaultAsync();
    }

    public async Task<OrderItem?> GetItemByIdAsync(Guid orderId, Guid productId)
    {
        return await _context.OrderItems
            .FirstOrDefaultAsync(item => 
                item.OrderId == orderId && 
                item.ProductId == productId);
    }

    public async Task AddOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task AddItemAsync(OrderItem item)
    {
        await _context.OrderItems.AddAsync(item);
    }

    public void RemoveItemAsync(OrderItem item)
    {
        _context.OrderItems.Remove(item);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}