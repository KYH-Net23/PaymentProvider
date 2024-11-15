using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentProvider.Models;
using PaymentProvider.Services;
using Stripe;
using Stripe.Checkout;

namespace PaymentProvider.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutController(OrderService orderService) : ControllerBase
    {
        private readonly OrderService _orderService = orderService;

        // api to get customer information and order details

        [HttpPost]
        public ActionResult Create([FromBody] OrderDetails orderDetails)
        {
            try
            {
                if (!ModelState.IsValid) BadRequest();
                if (orderDetails == null) return NotFound();
                orderDetails.OrderItemList = _orderService.GetOrderItemsList(orderDetails);
                var domain = "http://localhost:5173";
                var options = new SessionCreateOptions
                {
                    UiMode = "embedded",
                    Currency = "sek",
                    LineItems = orderDetails.OrderItemList,
                    Mode = "payment",
                    ReturnUrl = domain + "/return?session_id={CHECKOUT_SESSION_ID}",
                    CustomerEmail = orderDetails!.EmailAddress,
                    Metadata = new Dictionary<string, string>
                    {
                        {"orderId", $"{orderDetails.Id}" }
                    },
                };
                var service = new SessionService();
                var session = service.Create(options);
                return Ok(new { sessionId = session.Id, clientSecret = session.ClientSecret });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}