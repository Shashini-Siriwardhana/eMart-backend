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

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
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