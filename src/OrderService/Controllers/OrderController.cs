using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderService.DTOs;
using OrderService.Services;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrdersService _ordersService;

    public OrderController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetAllOrders(Guid userId)
    {
        var orders = await _ordersService.GetAllOrdersAsync(userId);
        return Ok(orders);
    }

    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var order = await _ordersService.GetOrderByIdAsync(orderId);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost("user/{userId:guid}")]
    public async Task<IActionResult> CreateOrder(Guid userId)
    {
        var order = await _ordersService.CreateOrderAsync(userId);

        if (order is null)
        {
            return NotFound();
        }

        return CreatedAtAction(nameof(GetOrderById), new {orderId = order.Id}, order);
    }

    [HttpPatch("{orderId:guid}")]
    public async Task<IActionResult> CancelOrder(Guid orderId)
    {
        var order = await _ordersService.CancelOrderAsync(orderId);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }
}