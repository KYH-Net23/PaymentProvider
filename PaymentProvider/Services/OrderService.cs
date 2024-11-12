using PaymentProvider.Models;
using Stripe.Checkout;
using Stripe.Climate;

namespace PaymentProvider.Services
{
    public class OrderService(HttpClient client)
    {
        private readonly HttpClient _client = client;
        private readonly List<ProductModel> _products =
        [
            new ProductModel { Id = 1, Model = "T-Shirt", Price = 1000, Quantity = 2 },
            new ProductModel { Id = 2, Model = "Pants", Price = 2000, Quantity = 1 }
        ];
        public async Task<OrderDetails?> GetOrderDetailsAsync(int id)
        {
            if (id == 1)
            {
                var order = new OrderDetails
                {
                    Id = 1,
                    EmailAddress = "xahit81459@anypng.com",
                    Products = _products,
                };
                order.OrderItemList = GetOrderItemsList(order);
                return order;
            }
            return null!;
        }

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
