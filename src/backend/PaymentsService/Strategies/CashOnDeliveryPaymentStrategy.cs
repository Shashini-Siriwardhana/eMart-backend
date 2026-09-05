namespace PaymentsService.Strategies;

public class CashOnDeliveryPaymentStrategy : IPaymentStrategy
{
    public async Task<bool> ProcessPaymentAsync(decimal amount)
    {
        // Simulate cash on delivery payment processing logic
        return await Task.FromResult(true);
    }
}