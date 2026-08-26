using Moq;
using ProductsService.Models;
using ProductsService.Services;
using ProductsService.Repositories;
using ProductsService.DTOs;

namespace ProductsService.Tests;

public class ProductsServiceTests
{
    [Fact]
    public async Task GetAllProductsAsync_ReturnsProducts_ProductExists()
    {

        // Arrange
        var expectedProducts = new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Macbook Air",
                Description = "13-inch laptop",
                Price = 375000,
                Category = "Laptop",
                ImageUrl= "",
                StockQuantity = 10
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Macbook Air M4",
                Description = "13-inch laptop",
                Price = 475000,
                Category = "Laptop",
                ImageUrl= "",
                StockQuantity = 5
            }
        };
        var mockRepository = new Mock<IProductRepository>();
        mockRepository
        .Setup(repo => repo.GetAllAsync())
        .ReturnsAsync(expectedProducts);

        var productService = new ProductService(mockRepository.Object);

        // Act
        var result = await productService.GetAllProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        mockRepository
        .Verify(
            repo => repo.GetAllAsync(),
            Times.Once
            );
    }

    [Fact]
    public async Task GetAllProductsAsync_NoProductsExist_ReturnsEmptyList()
    {

        // Arrange
        var expectedProducts = new List<Product>();
        var mockRepository = new Mock<IProductRepository>();
        mockRepository
        .Setup(repo => repo.GetAllAsync())
        .ReturnsAsync(expectedProducts);

        var productService = new ProductService(mockRepository.Object);

        // Act
        var result = await productService.GetAllProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        mockRepository
        .Verify(
            repo => repo.GetAllAsync(),
            Times.Once
            );
    }

    [Fact]
    public async Task GetProductByIdAsync_ProductExists_returnsProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var expectedProduct = new Product
        {
            Id = productId,
            Name = "Macbook Air",
            Description = "13-inch laptop",
            Price = 375000,
            Category = "Laptop",
            ImageUrl= "",
            StockQuantity = 10
        };
        var mockRepository = new Mock<IProductRepository>();
        mockRepository
        .Setup(repo => repo.GetByIdAsync(productId))
        .ReturnsAsync(expectedProduct);
        var productService = new ProductService(mockRepository.Object);

        // Act
        var result = await productService.GetProductByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal("Macbook Air", result.Name);
        Assert.Equal("13-inch laptop", result.Description);
        Assert.Equal(375000, result.Price);
        Assert.Equal("Laptop", result.Category);
        Assert.Equal("", result.ImageUrl);
        Assert.Equal(10, result.StockQuantity);

        mockRepository.Verify(
            repo => repo.GetByIdAsync(productId),
            Times.Once
        );
    }

    [Fact]
    public async Task GetProductByIdAsync_ProductDoesNotExists_returnsNull()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var mockRepository = new Mock<IProductRepository>();
        mockRepository
        .Setup(repo => repo.GetByIdAsync(productId))
        .ReturnsAsync((Product?)null);
        var productService = new ProductService(mockRepository.Object);

        // Act
        var result = await productService.GetProductByIdAsync(productId);

        // Assert
        Assert.Null(result);

        mockRepository.Verify(
            repo => repo.GetByIdAsync(productId),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateProductAsync_ProductCreated_ReturnsProduct()
    {
        var dto = new DTOs.CreateProductDto
        {
            Name = "Macbook Air",
            Description = "13-inch laptop",
            Price = 375000,
            Category = "Laptop",
            ImageUrl= "",
            StockQuantity = 10
        };
        var mockRepository = new Mock<IProductRepository>();
        mockRepository
        .Setup(repo => repo.AddAsync(It.IsAny<Product>()))
        .Returns(Task.CompletedTask);
        mockRepository
        .Setup(repo => repo.SaveChangesAsync())
        .Returns(Task.CompletedTask);
        var productService = new ProductService(mockRepository.Object);

        var result = await productService.CreateProductAsync(dto);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Description, result.Description);
        Assert.Equal(dto.Price, result.Price);
        Assert.Equal(dto.Category, result.Category);
        Assert.Equal(dto.ImageUrl, result.ImageUrl);
        Assert.Equal(dto.StockQuantity, result.StockQuantity);

        mockRepository.Verify(
            repo => repo.AddAsync(result),
            Times.Once
        );
        mockRepository.Verify(
            repo => repo.SaveChangesAsync(),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateProductAsync_ProductAlreadyExist_DoesNotCreateProduct()
    {
        var dto = new DTOs.CreateProductDto
        {
            Name = "Macbook Air",
            Description = "13-inch laptop",
            Price = 375000,
            Category = "Laptop",
            ImageUrl= "",
            StockQuantity = 10
        };
        var existingProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Macbook Air",
            Description = "13-inch laptop",
            Price = 375000,
            Category = "Laptop",
            ImageUrl= "",
            StockQuantity = 10
        };
        var mockRepository = new Mock<IProductRepository>();
        mockRepository
        .Setup(repo => repo.GetByNameAsync(dto.Name))
        .ReturnsAsync(existingProduct);
        var productService = new ProductService(mockRepository.Object);

        var result = await productService.CreateProductAsync(dto);

        Assert.Null(result);

        mockRepository.Verify(
            repo => repo.AddAsync(It.IsAny<Product>()),
            Times.Never
        );
        mockRepository.Verify(
            repo => repo.SaveChangesAsync(),
            Times.Never
        );
    }

    [Fact]
    public async Task UpdateProductAsync_ProductUpdated()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new Product
        {
            Id = productId,
            Name = "Macbook Air",
            Description = "13-inch laptop",
            Price = 375000,
            Category = "Laptop",
            ImageUrl= "",
            StockQuantity = 10
        };

        var dto = new UpdateProductDto
        {
            Name = "Mackbook Air"
        };
        var mockRepository = new Mock<IProductRepository>();
        mockRepository
        .Setup(repo => repo.GetByIdAsync(productId))
        .ReturnsAsync(existingProduct);

        var productService = new ProductService(mockRepository.Object);

        var result = await productService.UpdateProductAsync(productId, dto);

        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
        mockRepository.Verify(
            repo => repo.GetByIdAsync(productId),
            Times.Once
        );

        mockRepository.Verify(
            repo => repo.SaveChangesAsync(),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateProductAsync_ProductDoesNotExist_ProductDoesNotUpdated()
    {
        var productId = Guid.NewGuid();

        var dto = new UpdateProductDto
        {
            Name = "Mackbook Air"
        };
        var mockRepository = new Mock<IProductRepository>();
        
        mockRepository
        .Setup(repo => repo.GetByIdAsync(productId))
        .ReturnsAsync((Product?)null);

        var productService = new ProductService(mockRepository.Object);

        var result = await productService.UpdateProductAsync(productId, dto);

        Assert.Null(result);
        mockRepository.Verify(
            repo => repo.GetByIdAsync(productId),
            Times.Once
        );

        mockRepository.Verify(
            repo => repo.SaveChangesAsync(),
            Times.Never
        );
    }

    [Fact]
    public async Task DeleteProductAsync_ProductDeleted()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new Product
        {
            Id = productId,
            Name = "Macbook Air",
            Description = "13-inch laptop",
            Price = 375000,
            Category = "Laptop",
            ImageUrl= "",
            StockQuantity = 10
        };
        var mockRepository = new Mock<IProductRepository>();

        mockRepository
        .Setup(repo => repo.GetByIdAsync(existingProduct.Id))
        .ReturnsAsync(existingProduct);

        mockRepository
        .Setup(repo => repo.SaveChangesAsync())
        .Returns(Task.CompletedTask);

        var productService = new ProductService(mockRepository.Object);
        var result = await productService.DeleteProductAsync(existingProduct.Id);

        Assert.True(result);

        mockRepository.Verify(
            repo => repo.Remove(existingProduct),
            Times.Once
        );

        mockRepository.Verify(
            repo => repo.GetByIdAsync(existingProduct.Id),
            Times.Once
        );

        mockRepository.Verify(
            repo => repo.SaveChangesAsync(),
            Times.Once
        );
    }

    [Fact]
    public async Task DeleteProductAsync_ProductDoesNotExist_ProductNotDeleted()
    {
        var productId = Guid.NewGuid();

        var mockRepository = new Mock<IProductRepository>();

        mockRepository
        .Setup(repo => repo.GetByIdAsync(productId))
        .ReturnsAsync((Product?)null);

        mockRepository
        .Setup(repo => repo.SaveChangesAsync())
        .Returns(Task.CompletedTask);

        var productService = new ProductService(mockRepository.Object);
        var result = await productService.DeleteProductAsync(productId);

        Assert.False(result);

        mockRepository.Verify(
            repo => repo.Remove(It.IsAny<Product>()),
            Times.Never
        );

        mockRepository.Verify(
            repo => repo.GetByIdAsync(productId),
            Times.Once
        );

        mockRepository.Verify(
            repo => repo.SaveChangesAsync(),
            Times.Never
        );
    }
}