using ProductsService.Models;
using ProductsService.DTOs;

namespace ProductsService.Services;
public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product> CreateProductAsync(CreateProductDto product);
    Task<Product?> UpdateProductAsync(Guid id, UpdateProductDto product);
    Task<bool> DeleteProductAsync(Guid id);
}