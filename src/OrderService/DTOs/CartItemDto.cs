using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.DTOs;

public class CartItemDto
{
    public Guid ProductId {get; set;}

    [Range(1, int.MaxValue)]
    public int Quantity {get; set;}
    
}