using ProductsService.Models;
using ProductsService.DTOs;

namespace ProductsService.Services;
public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync(
        string? category = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? inStock = null,
        bool? orderByPriceAsc = null,
        int pageSize = 10,
        int pageNumber = 1
    );
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product> CreateProductAsync(CreateProductDto product);
    Task<Product?> UpdateProductAsync(Guid id, UpdateProductDto product);
    Task<bool> DeleteProductAsync(Guid id);
}