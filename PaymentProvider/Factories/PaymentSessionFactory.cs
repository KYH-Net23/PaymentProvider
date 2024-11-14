using PaymentProvider.Models;
using Stripe;
using Stripe.Checkout;

namespace PaymentProvider.Factories
{
    public static class PaymentSessionFactory
    {
        public static PaymentSession Create(Session session, int orderId, PaymentMethod paymentMethod, PaymentIntent paymentIntent)
        {
            return new PaymentSession
            {
                Session = session,
                OrderId = orderId,
                PaymentMethod = paymentMethod,
                PaymentIntent = paymentIntent
            };
        }
    }
}
