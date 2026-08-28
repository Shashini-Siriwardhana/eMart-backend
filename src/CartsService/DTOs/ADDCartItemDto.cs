using System.ComponentModel.DataAnnotations;

namespace CartsService.DTOs;

public class AddCartItemDto
{
    public Guid ProductId {get; set;}

    [Range(1, int.MaxValue)]
    public int Quantity {get; set;}
}