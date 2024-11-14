using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProvider.Models;
using PaymentProvider.Services;
using Stripe.Checkout;

namespace PaymentProvider.Controllers
{
    [Route("session-status")]
    [ApiController]
    public class SessionStatusController : ControllerBase
    {
        private readonly EmailService _emailService;

        public SessionStatusController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public ActionResult SessionStatus([FromQuery] string session_id)
        {
            try
            {
                var sessionService = new SessionService();
                var session = sessionService.Get(session_id);


                
                _emailService.SendEmail(session.CustomerEmail, session);

                return Ok(new
                {
                    status = session.Status,
                    customer_details = session.CustomerDetails,
                    line_items = session.LineItems,
                    payment_intent = session.PaymentIntent,
                    amount_total = session.AmountTotal,
                    invoice = session.Invoice
                });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
