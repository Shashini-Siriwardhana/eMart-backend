using CartsService.DTOs;

namespace CartsService.Clients;

public interface IProductApiClient
{
    Task<ProductDto?> GetProductByIdAsync(Guid productId);
}