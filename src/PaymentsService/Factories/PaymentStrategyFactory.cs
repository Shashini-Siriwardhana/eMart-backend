using PaymentsService.Enums;
using PaymentsService.Strategies;

namespace PaymentsService.Factories;

public class PaymentStrategyFactory : IPaymentStrategyFactory
{
    private readonly IServiceProvider _serviceProvicer;

    public PaymentStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvicer = serviceProvider;
    }
    public IPaymentStrategy GetPaymentStrategy(PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.Card => _serviceProvicer.GetRequiredService<CardPaymentStrategy>(),
            PaymentMethod.PayPal => _serviceProvicer.GetRequiredService<PayPalPaymentStrategy>(),
            PaymentMethod.CashOnDelivery => _serviceProvicer.GetRequiredService<CashOnDeliveryPaymentStrategy>(),
            _ => throw new NotSupportedException("Unsupported payment method")
        };
    }
}