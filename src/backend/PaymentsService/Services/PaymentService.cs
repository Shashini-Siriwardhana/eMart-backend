using PaymentsService.Clients;
using PaymentsService.DTOs;
using PaymentsService.Enums;
using PaymentsService.Factories;
using PaymentsService.Models;
using PaymentsService.Repositories;

namespace PaymentsService.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly IOrderApiClient _orderApiClient;
    private readonly IPaymentStrategyFactory _paymentStrategyFactory;

    public PaymentService(IPaymentRepository repository, IOrderApiClient orderApiClient, IPaymentStrategyFactory paymentStrategyFactory)
    {
        _repository = repository;
        _orderApiClient = orderApiClient;
        _paymentStrategyFactory = paymentStrategyFactory;
    }

    public async Task<Payment?> GetPaymentByOrderIdAsync(Guid orderId)
    {
        var payment = await _repository.GetPaymentByOrderIdAsync(orderId);

        return payment;
    }

    public async Task<List<Payment>> GetPaymentByUserIdAsync(Guid userId)
    {
        return await _repository.GetPaymentByUserIdAsync(userId);
    }

    public async Task<PaymentResult> CreatePaymentAsync(Guid orderId)
    {
        var order = await _orderApiClient.GetOrderAsync(orderId);

        if (order is null)
        {
            return new PaymentResult { IsSuccess = false, Message = "Order not found." };
        }

        if (order.Status != "Confirmed")
        {
            return new PaymentResult { IsSuccess = false, Message = "Order is not confirmed." };
        }

        var existingPayment = await _repository.GetPaymentByOrderIdAsync(orderId);
        
        if (existingPayment is not null)
        {
            return new PaymentResult 
            { 
                IsSuccess = false, 
                Message = "Payment already exists for this order.", 
                Payment = existingPayment 
            };
        }

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            UserId = order.UserId,
            Amount = order.TotalAmount,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow,

        };
        
        await _repository.CreatePaymentAsync(payment);
        await _repository.SaveChangesAsync();

        return new PaymentResult { IsSuccess = true, Message = "Payment created successfully.", Payment = payment };
    }

    public async Task<PaymentResult> UpdatePaymentAsync(Guid orderId, PaymentMethod paymentMethod)
    {
        var payment = await _repository.GetPaymentByOrderIdAsync(orderId);

        if (payment is null)
        {
            return new PaymentResult { IsSuccess = false, Message = "Payment not found." };
        }

        var strategy = _paymentStrategyFactory.GetPaymentStrategy(paymentMethod);
        var success = await strategy.ProcessPaymentAsync(payment.Amount);

        if (!success)
        {
            return new PaymentResult { IsSuccess = false, Message = "Payment processing failed." };
        }

        if (payment.Status == PaymentStatus.Successful)
        {
            return new PaymentResult 
            { 
                IsSuccess = false,
                Message = "Payment has already been processed successfully.",
                Payment = payment
                };
        }

        payment.Method = paymentMethod;
        payment.Status = PaymentStatus.Successful;
        payment.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdatePaymentAsync(payment);
        await _repository.SaveChangesAsync();
        return new PaymentResult { IsSuccess = true, Message = "Payment updated successfully.", Payment = payment };
    }
    
}