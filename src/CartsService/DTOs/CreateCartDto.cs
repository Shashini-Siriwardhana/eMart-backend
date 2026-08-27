using CartsService.Models;

namespace CartsService.DTOs;
public class CreateCartDto
{
    public AddCartItemDto CartItem {get; set;} = new();
}