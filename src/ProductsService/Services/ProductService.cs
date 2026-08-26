// using Microsoft.EntityFrameworkCore;
// using ProductsService.Data;
using ProductsService.Models;
using ProductsService.DTOs;
using ProductsService.Repositories;

namespace ProductsService.Services;
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

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
        return await _repository.GetAllAsync(
            category,
            minPrice,
            maxPrice,
            inStock,
            orderByPriceAsc,
            pageSize,
            pageNumber
        );
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Product?> CreateProductAsync(CreateProductDto productDto)
    {
        var existingProduct = await _repository.GetByNameAsync(productDto.Name);

        if (existingProduct is not null)
        {
            return null;
        }
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
        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateProductAsync(Guid id, UpdateProductDto productDto)
    {
        var existingProduct = await _repository.GetByIdAsync(id);
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

        await _repository.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        _repository.Remove(product);
        await _repository.SaveChangesAsync();
        return true;
    }
}