using PaymentProvider.Factories;
using PaymentProvider.Models;
using Stripe;

namespace PaymentProvider.Services
{
    public class StripeService()
    {
        private readonly CustomerService _service = new();

        public Customer CreateCustomer(OrderDetails order)
        {
            // check if customer already exists
            var existingCustomer = _service.List(new CustomerListOptions
            {
                Email = order.EmailAddress,
                Limit = 1
            }).FirstOrDefault();

            if (existingCustomer != null)
            {
                return existingCustomer;
            }

            var customerOptions = StripeCreateCustomerOptionsFactory.Create(order);
            var newCustomer = _service.Create(customerOptions);
            return newCustomer;
        }
        public Customer CreateCustomer(OrderModel order)
        {
            // check if customer already exists
            var existingCustomer = _service.List(new CustomerListOptions
            {
                Email = order.ReceivingEmail,
                Limit = 1
            }).FirstOrDefault();

            //if (existingCustomer != null)
            //{
            //    return existingCustomer;
            //}

            var customerOptions = StripeCreateCustomerOptionsFactory.Create(order);
            var newCustomer = _service.Create(customerOptions);
            return newCustomer;
        }
    }
}
