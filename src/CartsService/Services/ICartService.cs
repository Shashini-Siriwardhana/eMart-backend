using CartsService.DTOs;
using CartsService.Models;

namespace CartsService.Services;

public interface ICartService
{
    Task<Cart?> GetCartAsync(Guid userId);
    Task<Cart?> AddItemToCartAsync(Guid userId, AddCartItemDto dto);
    Task<Cart?> UpdateItemQuantityAsync(Guid userId, Guid productId, int quantity);
    Task<Cart?> DeleteItemFromCartAsync(Guid userId, Guid productId);
}