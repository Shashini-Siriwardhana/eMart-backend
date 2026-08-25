using Microsoft.EntityFrameworkCore;
using ProductsService.Data;
using ProductsService.Models;
using ProductsService.DTOs;

namespace ProductsService.Services;
public class ProductService : IProductService
{
    private readonly ProductDbContext _context;

    public ProductService(ProductDbContext context)
    {
        _context = context;
    }

    // Use LINQ to filter products based on the provided criteria
    /* IQueryable allows for deferred execution, meaning the query is not executed until the results are actually needed. 
    This allows building up a query with multiple conditions before executing it against the database.*/
    public async Task<List<Product>> GetAllProductsAsync(
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

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateProductAsync(CreateProductDto productDto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            Category = productDto.Category,
            ImageUrl = productDto.ImageUrl,
            StockQuantity = productDto.StockQuantity
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateProductAsync(Guid id, UpdateProductDto productDto)
    {
        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
        {
            return null;
        }

        if (productDto.Name != null) existingProduct.Name = productDto.Name;
        if (productDto.Description != null) existingProduct.Description = productDto.Description;
        if (productDto.Price.HasValue) existingProduct.Price = productDto.Price.Value;
        if (productDto.Category != null) existingProduct.Category = productDto.Category;
        if (productDto.ImageUrl != null) existingProduct.ImageUrl = productDto.ImageUrl;
        if (productDto.StockQuantity.HasValue) existingProduct.StockQuantity = productDto.StockQuantity.Value;

        await _context.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}