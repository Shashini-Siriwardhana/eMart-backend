using System.Net;
using OrderService.DTOs;

namespace OrderService.Clients;

public interface IProductApiClient
{
    Task<ProductDto?> GetProductByIdAsync(Guid productId);
}