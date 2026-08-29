namespace OrderService.DTOs;

public class UpdateOrderDto
{
    public Guid ProductId {get; set;}
    public int Quantity {get; set;}
}