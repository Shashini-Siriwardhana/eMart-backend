using System;
using System.Threading.Tasks;
using ProductsService.Models;

namespace ProductsService.Repositories;
public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(
        string? category = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? inStock = null,
        bool? orderByPriceAsc = null,
        int pageSize = 10,
        int pageNumber = 1
    );
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync(Product product);

    void Remove(Product product);
    Task SaveChangesAsync();
}