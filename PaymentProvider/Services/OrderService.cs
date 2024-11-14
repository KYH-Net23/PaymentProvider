using PaymentProvider.Models;
using Stripe.Checkout;
using Stripe.Climate;

namespace PaymentProvider.Services
{
    public class OrderService()
    {
        public List<SessionLineItemOptions> GetOrderItemsList(OrderDetails order)
        {
            var lineItems = new List<SessionLineItemOptions>();
            foreach (var product in order.Products)
            {
                order.TotalAmount += product.Price * product.Quantity;
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "sek",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Model,
                        },
                        UnitAmount = (long)product.Price,

                    },
                    Quantity = product.Quantity
                });
            }
            return lineItems;
        }
    }
}
