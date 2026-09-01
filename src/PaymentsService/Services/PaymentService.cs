using PaymentsService.Clients;
using PaymentsService.Enums;
using PaymentsService.Models;
using PaymentsService.Repositories;

namespace PaymentsService.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly IOrderApiClient _orderApiClient;

    public PaymentService(IPaymentRepository repository, IOrderApiClient orderApiClient)
    {
        _repository = repository;
        _orderApiClient = orderApiClient;
    }

    public async Task<Payment?> GetPaymentByOrderIdAsync(Guid orderId)
    {
        var payment = await _repository.GetPaymentByOrderIdAsync(orderId);

        if (payment is null)
        {
            return null;
        }

        return payment;
    }

    public async Task<List<Payment>> GetPaymentByUserIdAsync(Guid userId)
    {
        return await _repository.GetPaymentByUserIdAsync(userId);
    }

    public async Task<bool> CreatePaymentAsync(Guid orderId, PaymentMethod paymentMethod)
    {
        var order = await _orderApiClient.GetOrderAsync(orderId);

        if (order is null)
        {
            return false;
        }

        if (order.Status != "Confirmed")
        {
            return false;
        }

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            UserId = order.UserId,
            Amount = order.TotalAmount,
            Method = paymentMethod,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow,

        };
        await _repository.CreatePaymentAsync(payment);
        await _repository.SaveChangesAsync();
        return true;
    }
    
}