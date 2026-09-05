namespace PaymentsService.Strategies;

public interface IPaymentStrategy
{
    Task<bool> ProcessPaymentAsync(decimal amount);
}