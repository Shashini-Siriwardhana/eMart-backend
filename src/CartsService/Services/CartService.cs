using CartsService.Clients;
using CartsService.DTOs;
using CartsService.Models;
using CartsService.Repositories;

namespace CartsService.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _repository;
    private readonly IProductApiClient _productApiClient;

    public CartService (ICartRepository repository, IProductApiClient productApiClient)
    {
        _repository = repository;
        _productApiClient = productApiClient;
    }

    public async Task<Cart?> GetCartAsync(Guid userId)
    {
        var cart = await _repository.GetCartByUserIdAsync(userId);
        if (cart is null)
        {
            return cart;
        }
        return await CreateCartResponse(cart);
    }

    public async Task<Cart?> AddItemToCartAsync(Guid userId, AddCartItemDto dto)
    {
        var product = await _productApiClient.GetProductByIdAsync(dto.ProductId);

        if (product is null)
        {
            return null;
        }

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
            return await CreateCartResponse(existingCart);
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
        return await CreateCartResponse(cart);
    }

    public async Task<Cart?> UpdateItemQuantityAsync(Guid userId, Guid productId, int quantity)
    {
        var product = await _productApiClient.GetProductByIdAsync(productId);

        if (product is null)
        {
            return null;
        }

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

        if (updatedQuantity < 0 || updatedQuantity > product.StockQuantity)
        {
            return null;
        } else if (updatedQuantity == 0)
        {
            _repository.RemoveCartItem(existingCartItem);
        }
        await _repository.UpdateItemQuantity(existingCart.Id, productId, updatedQuantity);
        existingCart.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync();

        return await CreateCartResponse(existingCart);
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
        return await CreateCartResponse(existingCart);
    }

    private async Task<Cart> CreateCartResponse(Cart cart)
    {
        foreach (var cartItem in cart.CartItems)
        {
            var product = await _productApiClient
                .GetProductByIdAsync(cartItem.ProductId);

            if (product is null)
            {
                continue;
            }
            cartItem.ProductName = product.Name;
            cartItem.Price = product.Price;
            cartItem.ImageUrl = product.ImageUrl;
            cartItem.Subtotal = product.Price * cartItem.Quantity;
        }

        return cart;
    }

}