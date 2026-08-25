using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ProductsService.Data;
using ProductsService.Models;

namespace ProductsService.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _context;

    public ProductRepository(ProductDbContext context)
    {
        _context = context;
    }

    // Use LINQ to filter products based on the provided criteria
    /* IQueryable allows for deferred execution, meaning the query is not executed until the results are actually needed. 
    This allows building up a query with multiple conditions before executing it against the database.*/
    public async Task<List<Product>> GetAllAsync(
        string? category = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? inStock = null,
        bool? orderByPriceAsc = null,
        int pageSize = 10,
        int pageNumber = 1
    )
    {
        var query = _context.Products.AsQueryable();
        
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.Category == category);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        if (inStock.HasValue && inStock.Value )
        {
            query = query.Where(p => p.StockQuantity > 0);
        }

        if (orderByPriceAsc.HasValue && orderByPriceAsc.Value)
        {
            query = query.OrderBy(p => p.Price); // Sort by price in ascending order
        }

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return await query.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void Remove(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}