using CartsService.DTOs;
using CartsService.Models;
using CartsService.Repositories;

namespace CartsService.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _repository;

    public CartService (ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task<Cart?> GetCartAsync(Guid userId)
    {
        return await _repository.GetCartByUserIdAsync(userId);
    }

    public async Task<Cart?> AddItemToCartAsync(Guid userId, AddCartItemDto dto)
    {
        var existingCart = await _repository.GetCartByUserIdAsync(userId);

        if (existingCart is not null)
        {
            var existingItem = existingCart.CartItems
            .FirstOrDefault(item => item.ProductId == dto.ProductId);

            if (existingItem is null)
            {
                CartItem item = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = existingCart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };
                await _repository.AddCartItemAsync(item);
            } else
            {
                var updatedQuantity = existingItem.Quantity + dto.Quantity;
                existingItem.Quantity = updatedQuantity < 0 ? existingItem.Quantity : updatedQuantity;
            }

            existingCart.UpdatedAt = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
            return existingCart;
        }

        Guid cartId = Guid.NewGuid();

        CartItem cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cartId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

        Cart cart = new Cart
        {
            Id = cartId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            CartItems = [cartItem]
        };

        await _repository.AddCartAsync(cart);
        await _repository.SaveChangesAsync();
        return cart;
    }

    public async Task<Cart?> UpdateItemQuantityAsync(Guid userId, Guid productId, int quantity)
    {
        var existingCart = await _repository.GetCartByUserIdAsync(userId);

        if (existingCart is null)
        {
            return null;
        }

        var existingCartItem = existingCart.CartItems
        .FirstOrDefault(item =>
            item.ProductId == productId);

        if (existingCartItem is null)
        {
            return null;
        }

        var updatedQuantity = existingCartItem.Quantity + quantity;

        if (updatedQuantity < 0)
        {
            return null;
        } else if (updatedQuantity == 0)
        {
            _repository.RemoveCartItem(existingCartItem);
        }
        await _repository.UpdateItemQuantity(existingCart.Id, productId, updatedQuantity);
        existingCart.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync();

        return existingCart;
    }

    public async Task<Cart?> DeleteItemFromCartAsync(Guid userId, Guid productId)
    {
        var existingCart = await _repository.GetCartByUserIdAsync(userId);

        if (existingCart is null)
        {
            return null;
        }

        var cartItem = existingCart.CartItems.FirstOrDefault(item => item.ProductId == productId);

        if (cartItem is null)
        {
            return null;
        }

        _repository.RemoveCartItem(cartItem);
        existingCart.CartItems.Remove(cartItem);
        existingCart.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync();
        return existingCart;
    }
}