using PaymentsService.Enums;
using PaymentsService.Strategies;

namespace PaymentsService.Factories;

public interface IPaymentStrategyFactory
{
    IPaymentStrategy GetPaymentStrategy(PaymentMethod paymentMethod);
}