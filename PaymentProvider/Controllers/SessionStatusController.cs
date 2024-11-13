using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProvider.Models;
using PaymentProvider.Services;
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
                if (session.PaymentStatus == "paid")
                {
                    var emailTask = await _emailService.SendEmailAsync(session.CustomerEmail, session, new OrderDetails { Id = 1 });
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
