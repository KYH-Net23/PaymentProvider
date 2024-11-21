using Microsoft.AspNetCore.Mvc;
using PaymentProvider.Models;
using PaymentProvider.Services;
using Stripe.Checkout;

namespace PaymentProvider.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutController(OrderService orderService, StripeService stripeCustomerService) : ControllerBase
    {
        private readonly OrderService _orderService = orderService;
        private readonly StripeService _stripeCustomerService = stripeCustomerService;

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
                var customer = _stripeCustomerService.CreateCustomer(orderDetails);
                var options = new SessionCreateOptions
                {
                    UiMode = "embedded",
                    Currency = "sek",
                    LineItems = orderDetails.OrderItemList,
                    Mode = "payment",
                    ReturnUrl = domain + "/return?session_id={CHECKOUT_SESSION_ID}",
                    //CustomerEmail = orderDetails!.EmailAddress,
                    Customer = customer.Id,
                    Metadata = new Dictionary<string, string>
                    {
                        {"orderId", $"{orderDetails.Id}" }
                    },
                    ShippingOptions =
                    [
                        _orderService.GetShippingOption(orderDetails.ServicePoint, orderDetails.DeliveryOption)
                    ],
                    InvoiceCreation = new SessionInvoiceCreationOptions
                    {
                        Enabled = true
                    },
                    BillingAddressCollection = "required",

                };
                var service = new SessionService();
                var session = service.Create(options);
                session.Customer = customer;
                return Ok(new { sessionId = session.Id, clientSecret = session.ClientSecret });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
    }
}