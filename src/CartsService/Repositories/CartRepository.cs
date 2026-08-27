using System.ComponentModel;
using CartsService.Data;
using CartsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CartsService.Repositories;

public class CartRepository : ICartRepository
{
    private readonly CartDbContext _context;

    public CartRepository(CartDbContext context) {
        _context = context;
    }

    public async Task<Cart?> GetCartByCartIdAsync(Guid cartId)
    {
        var result = await _context.Carts.FindAsync(cartId);
        return result;
    }

    public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
    {
        var result = await _context.Carts
        .Include(Cart => Cart.CartItems)
        .FirstOrDefaultAsync(cart => cart.UserId == userId);
        return result;
    }

    public async Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId)
    {
        var result = await _context.CartItems.FirstOrDefaultAsync(item => item.CartId == cartId && item.ProductId == productId);
        return result;
    }

    public async Task AddCartAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
    }

    public async Task AddCartItemAsync(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
    }

    public async Task UpdateItemQuantity(Guid cartId, Guid productId, int quantity)
    {
        var item = await _context.CartItems.FirstOrDefaultAsync(item => item.CartId == cartId && item.ProductId == productId);

        if (item is not null) 
        {
            item.Quantity = quantity;
        }
    }

    public void RemoveCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}