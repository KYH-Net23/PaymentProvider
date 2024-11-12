using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProvider.Models;
using Stripe;
using Stripe.Checkout;

namespace PaymentProvider.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutApiController : ControllerBase
    {
        // api to get customer information and order details
        // retrieve Price ID from order details - find way to not have to use price id?

        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                var products = new List<ProductModel>()
                {
                    new ProductModel { Id = 1, Model = "T-Shirt", Price = 1000, Quantity = 2 },
                    new ProductModel { Id = 2, Model = "Pants", Price = 2000, Quantity = 1 }
                };
                var order = new OrderDetails { Id = 1, Products = products };
                var lineItems = new List<SessionLineItemOptions>();
                foreach (var product in products)
                {
                    order.TotalAmount += product.Price * product.Quantity;
                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "sek",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = product.Model
                            },
                            UnitAmount = (long)product.Price
                        },
                        Quantity = product.Quantity
                    });
                }
                var domain = "http://localhost:5173";
                var options = new SessionCreateOptions
                {
                    UiMode = "embedded",
                    LineItems = lineItems,
                    Mode = "payment",
                    ReturnUrl = domain + "/return?session_id={CHECKOUT_SESSION_ID}",
                    CustomerEmail = order.EmailAddress,
                };
                var service = new SessionService();
                var session = service.Create(options);

                return Ok(new { clientSecret = session.ClientSecret });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
