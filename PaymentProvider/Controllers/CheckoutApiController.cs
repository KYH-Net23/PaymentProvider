using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                var domain = "http://localhost:3000";
                var options = new SessionCreateOptions
                {
                    UiMode = "embedded",
                    LineItems =
                    [
                        new() {
                            Price = "price_1QJu0VKTnkBH3a68HwDSxqdH",
                            Quantity = 1,
                        }
                    ],
                    PaymentMethodTypes = ["card"],
                    Mode = "payment",
                    ReturnUrl = domain + "/return?session_id={CHECKOUT_SESSION_ID}"
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
