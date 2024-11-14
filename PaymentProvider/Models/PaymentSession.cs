using Stripe;
using Stripe.Checkout;

namespace PaymentProvider.Models
{
    public class PaymentSession
    {
        public Session Session { get; set; } = null!;
        public int OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = null!;
        private string? _paymentMethodInfo;
        public string PaymentMethodInfo
        {
            get
            {
                if (_paymentMethodInfo == null)
                {
                    switch (PaymentMethod.Type)
                    {
                        case "card":
                            _paymentMethodInfo = $" ending with {PaymentMethod.Card.Last4}";
                            break;
                        case "klarna":
                            _paymentMethodInfo = string.Empty;
                            break;
                        case "paypal":
                            _paymentMethodInfo = PaymentMethod.Paypal.PayerEmail;
                            break;
                    }
                }
                return _paymentMethodInfo!;
            }
            set
            {
                _paymentMethodInfo = value;
            }
        }
        public PaymentIntent PaymentIntent { get; set; } = null!;
    }


}
