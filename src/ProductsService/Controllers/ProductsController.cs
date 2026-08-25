using Microsoft.AspNetCore.Mvc;
using ProductsService.DTOs;
using ProductsService.Services;

namespace ProductsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] string? category = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] bool? inStock = null,
        [FromQuery] bool? orderByPriceAsc = null,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1
    )
    {
        var products = await _productService.GetAllProductsAsync(
            category,
            minPrice,
            maxPrice,
            inStock,
            orderByPriceAsc,
            pageSize,
            pageNumber
        );
        return Ok(products);    
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto product)
    {
        var createdProduct = await _productService.CreateProductAsync(product);

        if (createdProduct is null)
        {
            return Conflict(new
            {
                message = "A product with this name already exists."
            });
        }
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDto product)
    {
        var updatedProduct = await _productService.UpdateProductAsync(id, product);
        if (updatedProduct == null)
        {
            return NotFound();
        }

        return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var deletedProduct = await _productService.DeleteProductAsync(id);
        if (!deletedProduct)
        {
            return NotFound();
        }
        return Ok(new
        {
            message = "Product deleted successfully",
            id = id
        });
    }
}