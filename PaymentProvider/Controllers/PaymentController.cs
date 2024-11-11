using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProvider.Models;
using Stripe;
using Stripe.Checkout;
using Stripe.Forwarding;

namespace PaymentProvider.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        [HttpPost]
        public IActionResult ProcessPayment([FromBody] PaymentRequest paymentRequest)
        {
            try
            {

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)paymentRequest.TotalAmount,
                    Currency = "sek",
                    PaymentMethod = paymentRequest.PaymentMethod,
                    ConfirmationMethod = "automatic",
                    Confirm = true,
                    ReturnUrl = "http://localhost:5173/",

                };
                var service = new PaymentIntentService();
                var paymentIntent = service.Create(options);

                if (paymentIntent.Status == "succeeded")
                {
                    return Ok(new { success = true, message = "Payment processed successfully" });
                }
                return BadRequest(new { success = false, message = "Payment failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}
