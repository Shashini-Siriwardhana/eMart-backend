using PaymentsService.Enums;
using PaymentsService.Models;

namespace PaymentsService.Services;

public interface IPaymentService
{
    Task<Payment?> GetPaymentByOrderIdAsync(Guid orderId);
    Task<List<Payment>> GetPaymentByUserIdAsync(Guid userId);
    Task<bool> CreatePaymentAsync(Guid orderId, PaymentMethod paymentMethod);
}