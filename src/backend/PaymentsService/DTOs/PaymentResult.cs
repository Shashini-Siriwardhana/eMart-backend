using PaymentsService.Models;

namespace PaymentsService.DTOs;

public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public Payment? Payment { get; set; }
}