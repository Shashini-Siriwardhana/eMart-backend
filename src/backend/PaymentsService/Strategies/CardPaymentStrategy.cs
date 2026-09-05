namespace PaymentsService.Strategies;

public class CardPaymentStrategy : IPaymentStrategy
{
    public async Task<bool> ProcessPaymentAsync(decimal amount)
    {
        // Simulate card payment processing logic
        return await Task.FromResult(true);
    }
}