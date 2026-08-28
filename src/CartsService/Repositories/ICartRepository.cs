using CartsService.Models;

namespace CartsService.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetCartByCartIdAsync(Guid cartId);
    Task<Cart?> GetCartByUserIdAsync(Guid userId);
    Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId);
    Task AddCartAsync(Cart cart);
    Task AddCartItemAsync(CartItem cartItem);
    Task UpdateItemQuantity(Guid cartId, Guid productId, int quantity);
    void RemoveCartItem(CartItem cartItem);
    Task SaveChangesAsync();
}