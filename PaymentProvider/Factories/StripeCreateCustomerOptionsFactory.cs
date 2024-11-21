using PaymentProvider.Models;
using PaymentProvider.Models.OrderConfirmationModels;
using Stripe;

namespace PaymentProvider.Factories
{
    public static class StripeCreateCustomerOptionsFactory
    {
        public static CustomerCreateOptions Create(OrderModel order)
        {
            return new CustomerCreateOptions
            {
                Address = new AddressOptions
                {
                    Line1 = order.Shipping.CustomerDeliveryInformation.StreetAddress,
                    City = order.Shipping.CustomerDeliveryInformation.City,
                    Country = order.Shipping.CustomerDeliveryInformation.Country,
                    PostalCode = order.Shipping.CustomerDeliveryInformation.PostalCode,
                    State = order.Shipping.CustomerDeliveryInformation.City
                },
                Shipping = new ShippingOptions
                {
                    Name = order.Shipping.CustomerDeliveryInformation.FullName,
                    Address = new AddressOptions
                    {
                        Line1 = order.Shipping.CustomerDeliveryInformation.StreetAddress,
                        City = order.Shipping.CustomerDeliveryInformation.City,
                        Country = order.Shipping.CustomerDeliveryInformation.Country,
                        PostalCode = order.Shipping.CustomerDeliveryInformation.PostalCode,
                        State = order.Shipping.CustomerDeliveryInformation.City
                    },
                    Phone = order.Shipping.CustomerDeliveryInformation.PhoneNumber
                },
                Email = order.ReceivingEmail,
                Phone = order.Shipping.CustomerDeliveryInformation.PhoneNumber,
                Name = order.Shipping.CustomerDeliveryInformation.FullName,
            };
        }
    }
}
