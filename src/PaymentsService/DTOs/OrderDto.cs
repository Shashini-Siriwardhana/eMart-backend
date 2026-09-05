using PaymentsService.Enums;

namespace PaymentsService.DTOs;

public class OrderDto
{
    public Guid Id {get; set;}
    public Guid UserId {get; set;}
    public string Status {get; set;} = string.Empty;
    public decimal TotalAmount {get; set;}
}