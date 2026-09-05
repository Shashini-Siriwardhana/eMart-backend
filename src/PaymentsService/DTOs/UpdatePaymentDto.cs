using PaymentsService.Enums;

namespace PaymentsService.DTOs;

public class UpdatePaymentDto
{
    public Guid OrderId {get; set;}
    public PaymentMethod PaymentMethod {get; set;}
}