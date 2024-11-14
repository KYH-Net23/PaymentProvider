using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProvider.Models;
using PaymentProvider.Services;
using Stripe;
using Stripe.Checkout;

namespace PaymentProvider.Controllers
{
    [Route("session-status")]
    [ApiController]
    public class SessionStatusController(EmailService emailService) : ControllerBase
    {
        private readonly EmailService _emailService = emailService;

        [HttpGet]
        public async Task<ActionResult> SessionStatus([FromQuery] string session_id)
        {
            try
            {

                var sessionService = new SessionService();
                var session = sessionService.Get(session_id);
                var sessionLineItemService = new SessionLineItemService();
                var lineItems = sessionLineItemService.List(session_id);
                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = paymentIntentService.Get(session.PaymentIntentId);
                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = paymentMethodService.Get(paymentIntent.PaymentMethodId);

                session.LineItems = lineItems;
                if (session.PaymentStatus == "paid")
                {
                    var paymentSession = new PaymentSession
                    {
                        Session = session,
                        OrderId = int.Parse(session.Metadata["orderId"]),
                        PaymentIntent = paymentIntent,
                        PaymentMethod = paymentMethod,
                    };
                    var emailTask = await _emailService.SendEmailAsync(session.CustomerEmail, paymentSession);
                    return Ok(new { emailSent = emailTask, status = session.Status });
                }
                return BadRequest(new { status = session.Status });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
