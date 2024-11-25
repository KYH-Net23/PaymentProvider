﻿using Microsoft.AspNetCore.Mvc;
using PaymentProvider.Entities;
using PaymentProvider.Factories;
using PaymentProvider.Services;
using Stripe;
using Stripe.Checkout;

namespace PaymentProvider.Controllers
{
    [Route("session-status")]
    [ApiController]
    public class SessionStatusController(EmailService emailService, OrderService orderService, InvoiceRequestService invoiceRequestService) : ControllerBase
    {
        private readonly EmailService _emailService = emailService;
        private readonly OrderService _orderService = orderService;
        private readonly InvoiceRequestService _invoiceRequestService = invoiceRequestService;

        [HttpGet]
        public async Task<ActionResult> SessionStatus([FromQuery] string session_id)
        {
            try
            {
                var sessionService = new SessionService();
                var session = sessionService.Get(session_id);
                var sessionLineItemService = new SessionLineItemService();
                var lineItems = sessionLineItemService.List(session_id);

                var customerService = new CustomerService();
                var customer = customerService.Get(session.CustomerId);

                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = paymentMethodService.Get(paymentIntent.PaymentMethodId);

                var invoiceService = new InvoiceService();
                var invoice = invoiceService.Get(paymentIntent.InvoiceId);

                session.Invoice = invoice;
                session.Customer = customer;
                session.PaymentIntent = paymentIntent;
                session.PaymentIntent.PaymentMethod = paymentMethod;

                var order = await _orderService.GetAsync(session_id);
                order.Invoice ??= new InvoiceEntity();
                order.Invoice.City = session.Invoice.CustomerAddress.City ?? "";
                order.Invoice.Country = session.Invoice.CustomerAddress.Country ?? "";
                order.Invoice.FullName = session.Invoice.CustomerName ?? "";
                order.Invoice.PaymentOption = session.PaymentIntent.PaymentMethod.Card.Brand ?? "";
                order.Invoice.PostalCode = session.Invoice.CustomerAddress.PostalCode ?? "";
                order.Invoice.StreetAddress = session.Invoice.CustomerAddress.Line1 ?? "";
                order.Invoice.InvoiceUrl = session.Invoice.InvoicePdf ?? "";

                session.LineItems = lineItems;

                if (session.PaymentStatus == "paid")
                {
                    var orderConfirmation = OrderConfirmationModelFactory.Create(order);
                    if (await _emailService.SendEmailInformationAsync(orderConfirmation))
                    {
                        await _orderService.SaveChangesAsync();
                        //var dbInvoice = new InvoiceRequest
                        //{
                        //    Amount = order.OrderTotal,
                        //    CustomerId = 0,
                        //    Date = DateTime.UtcNow,
                        //    DueDate = DateTime.UtcNow.AddDays(7),
                        //    OrderId = order.Id,
                        //    PaidDate = DateTime.UtcNow,
                        //    Status = "paid"
                        //};
                        //await _invoiceRequestService.CreateInvoiceAsync(dbInvoice);
                        return Ok(new { session, orderConfirmation, status = session.Status, customer_email = session.CustomerEmail ?? session.Customer.Email, orderId = order.Id });
                    }
                    await _orderService.Delete(order);
                    return BadRequest();
                }
                return BadRequest(new { status = session.Status });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}