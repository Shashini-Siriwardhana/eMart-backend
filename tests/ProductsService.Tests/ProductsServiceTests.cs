using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProductsService.Data;
using ProductsService.Models;
using ProductsService.Services;

namespace ProductsService.Tests;

public class ProductsServiceTests
{
    [Fact]
    public async Task GetAllProductsAsync_ReturnsNull_WhenNoProductsExist()
    {
        // Arrange
        var (connection, context) = await CreateTestContextAsync();

        await using (connection)
        await using (context)
        {
            // Create actual class we're testing
            var productsService = new ProductService(context);

            // Act
            var result = await productsService.GetAllProductsAsync();

            // Assert
            Assert.Empty(result);
        }
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsProducts_ProductExists()
    {
        var (connection, context) = await CreateTestContextAsync();

        await using (connection)
        await using (context)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Macbook Air",
                Description = "13-inch laptop",
                Price = 375000,
                Category = "Laptop",
                ImageUrl= "",
                StockQuantity = 10
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var productService = new ProductService(context);

            var result = await productService.GetAllProductsAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsNull_WhenNoProductExist()
    {
        var (connection, context) = await CreateTestContextAsync();

        await using (connection)
        await using (context)
        {
            var productService = new ProductService(context);
            var productId = Guid.NewGuid();
            var result = await productService.GetProductByIdAsync(productId);

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsProduct_WhenProductExist()
    {
        var (connection, context) = await CreateTestContextAsync();
        await using (connection)
        await using (context)
        {
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Macbook Air",
                Description = "13-inch laptop",
                Price = 375000,
                Category = "Laptop",
                ImageUrl= "",
                StockQuantity = 10
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            var productService = new ProductService(context);

            var result = await productService.GetProductByIdAsync(productId);

            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Macbook Air", result.Name);
            Assert.Equal("13-inch laptop", result.Description);
            Assert.Equal(375000, result.Price);
            Assert.Equal("Laptop", result.Category);
            Assert.Equal("", result.ImageUrl);
            Assert.Equal(10, result.StockQuantity);
        }
    }

    private async Task<(SqliteConnection connection, ProductDbContext context)> CreateTestContextAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        // Create configuration for the existing ProductDbContext using SQLite instead PostgreSQL
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseSqlite(connection)
            .Options;

        // Create database context
        var context = new ProductDbContext(options);
        // Create tables required my model
        await context.Database.EnsureCreatedAsync();
        return (connection, context);
    }
}