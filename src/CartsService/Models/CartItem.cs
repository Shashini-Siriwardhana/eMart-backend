using System.ComponentModel.DataAnnotations;

namespace CartsService.Models;

public class CartItem
{
    public Guid Id {get; set;}

    public Guid CartId {get; set;}

    public Guid ProductId {get; set;}

    [Range(1, int.MaxValue)]
    public int Quantity {get; set;}
}