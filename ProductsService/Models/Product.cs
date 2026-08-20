using System.ComponentModel.DataAnnotations;

namespace ProductsService.Models;
public class Product
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public required decimal Price { get; set; }

    [Required]
    [StringLength(100)]
    public required string Category { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
    
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
    public required int StockQuantity { get; set; }
}