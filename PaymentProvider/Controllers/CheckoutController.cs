using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentProvider.Converters;
using PaymentProvider.Models;
using PaymentProvider.Services;
using Stripe;
using Stripe.Checkout;

namespace PaymentProvider.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutController(HttpClient client) : ControllerBase
    {
        private readonly HttpClient _client = client;

        // api to get customer information and order details
        // retrieve Price ID from order details - find way to not have to use price id?

        [HttpPost("{orderId}")]
        public async Task<ActionResult> Create(int orderId)
        {
            try
            {
                var response = await _client.GetAsync($"https://localhost:7127/api/Order/{orderId}");
                var order = new OrderDetails();
                if (response.IsSuccessStatusCode)
                {
                    var settings = new JsonSerializerSettings();
                    settings.Converters.Add(new SessionLineItemOptionsConverter());
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    order = JsonConvert.DeserializeObject<OrderDetails>(jsonResponse, settings);
                    if (order == null) return BadRequest(jsonResponse);
                }
                var domain = "http://localhost:5173";
                var options = new SessionCreateOptions
                {
                    UiMode = "embedded",
                    Currency = "sek",
                    LineItems = order.OrderItemList,
                    Mode = "payment",
                    ReturnUrl = domain + "/return?session_id={CHECKOUT_SESSION_ID}",
                    CustomerEmail = order!.EmailAddress,
                };
                var service = new SessionService();
                var session = service.Create(options);
                return Ok(new { clientSecret = session.ClientSecret });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
