using CartsService.DTOs;
using CartsService.Models;
using CartsService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetCartItems(Guid userId)
    {
        var cart = await _cartService.GetCartAsync(userId);

        if (cart is null)
        {
            return NotFound();
        }

        return Ok(cart);
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> CreateCartItem(Guid userId, [FromBody] AddCartItemDto addCartItemDto)
    {
        var cart = await _cartService.AddItemToCartAsync(userId, addCartItemDto);

        if (cart is null)
        {
            return NotFound();
        }

        return Ok(cart);
    }

    [HttpPatch("{userId:guid}/items/{productId:guid}")]
    public async Task<IActionResult> UpdateCart(Guid userId, Guid productId, [FromBody] UpdateCartDto updateCartDto)
    {
        var cart = await _cartService.UpdateItemQuantityAsync(userId, productId, updateCartDto.Quantity);

        if (cart is null)
        {
            return NotFound();
        }

        return Ok(cart);
    }

    [HttpDelete("{userId:guid}/items/{productId:guid}")]
    public async Task<IActionResult> DeleteItemFromCart(Guid userId, Guid productId)
    {
        var cart = await _cartService.DeleteItemFromCartAsync(userId, productId);

        if (cart is null)
        {
            return NotFound();
        }

        return Ok(cart);
    }
    
}