using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartsService.Models;

public class CartItem
{
    public Guid Id {get; set;}

    public Guid CartId {get; set;}

    public Guid ProductId {get; set;}

    [Range(1, int.MaxValue)]
    public int Quantity {get; set;}
    
    [NotMapped]
    public string? ProductName { get; set; }

    [NotMapped]
    public decimal? Price { get; set; }

    [NotMapped]
    public string? ImageUrl { get; set; }

    [NotMapped]
    public decimal? Subtotal { get; set; }
}