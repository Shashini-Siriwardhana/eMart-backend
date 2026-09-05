namespace PaymentsService.Strategies;

public class PayPalPaymentStrategy : IPaymentStrategy
{
    public async Task<bool> ProcessPaymentAsync(decimal amount)
    {
        // Simulate paypal payment processing logic
        return await Task.FromResult(true);
    }
}