using PaymentProvider.Models;
using PaymentProvider.Models.OrderConfirmationModels;
using Stripe;

namespace PaymentProvider.Factories
{
    public static class StripeCreateCustomerOptionsFactory
    {
        public static CustomerCreateOptions Create(OrderDetails order)
        {
            return new CustomerCreateOptions
            {
                Address = new AddressOptions
                {
                    Line1 = order.Address,
                    Line2 = order.Address2,
                },
                Email = order.EmailAddress,
                Name = order.Name,
                Phone = order.PhoneNumber
            };
        }
        public static CustomerCreateOptions Create(OrderConfirmationModel order)
        {
            return new CustomerCreateOptions
            {
                Shipping = new ShippingOptions
                {
                    Name = order.Shipping.FullName,
                    Address = new AddressOptions
                    {
                        Line1 = order.Shipping.CustomerDeliveryAddress
                    },
                    Phone = order.Shipping.PhoneNumber
                },
                Email = order.ReceivingEmail,
                PaymentMethod = order.Invoice.PaymentOption,
                Phone = order.Shipping.PhoneNumber,
                Name = order.Shipping.FullName,
            };
        }
    }
}
