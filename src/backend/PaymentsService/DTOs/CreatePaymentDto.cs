using PaymentsService.Enums;

namespace PaymentsService.DTOs;

public class CreatePaymentDto
{
    public Guid OrderId {get; set;}
}