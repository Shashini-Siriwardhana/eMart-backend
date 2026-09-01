using Microsoft.EntityFrameworkCore;
using PaymentsService.Data;
using PaymentsService.DTOs;
using PaymentsService.Enums;
using PaymentsService.Models;
using PaymentsService.Services;

namespace PaymentsService.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;
    public PaymentRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetPaymentByOrderIdAsync(Guid orderId)
    {
        return await _context.Payments.FirstOrDefaultAsync(payment => payment.OrderId == orderId);
    }

    public async Task<List<Payment>> GetPaymentByUserIdAsync(Guid userId)
    {
        return await _context.Payments.Where(payment => payment.UserId == userId).ToListAsync();
    }

    public async Task CreatePaymentAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}