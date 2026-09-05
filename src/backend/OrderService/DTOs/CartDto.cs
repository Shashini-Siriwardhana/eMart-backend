namespace OrderService.DTOs;

public class CartDto
{
    public Guid Id {get; set;}

    public Guid UserId {get; set;}

    public DateTime CreatedAt {get; set;}

    public DateTime? UpdatedAt {get; set;}

    public List<CartItemDto> CartItems {get; set;} = new();
}