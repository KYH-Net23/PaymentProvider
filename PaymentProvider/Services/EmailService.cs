using Azure;
using Azure.Communication.Email;
using PaymentProvider.Factories;
using PaymentProvider.Models;
using PaymentProvider.Repositories;
using Stripe;
using Stripe.Checkout;

namespace PaymentProvider.Services
{
    public class EmailService(EmailSessionRepository sessionRepository, string connectionString)
    {
        private readonly EmailClient _emailClient = new(connectionString);
        private readonly EmailSessionRepository _sessionRepository = sessionRepository;
        private readonly string _senderAddress = "DoNotReply@e610b531-2626-468a-b39c-ee360d0cb912.azurecomm.net";

        public async Task<bool> IsEmailSent(string sessionId)
        {
            var session = await _sessionRepository.GetEmailSessionAsync(sessionId);

            if (session != null)
                return session.Sent;
            
            return false;
        }

        public async Task<bool> SendEmailAsync(string toAddress, PaymentSession paymentSession)
        {
            try
            {
                var emailContent = GenerateEmailContent(paymentSession);

                if (string.IsNullOrEmpty(emailContent)) return false;

                var emailSession = await _sessionRepository.GetEmailSessionAsync(paymentSession.Session.Id);

                if (emailSession == null)
                {      
                    emailSession = EmailSessionFactory.Create(paymentSession.Session.Id, paymentSession.OrderId, false, DateTime.UtcNow);
                    await _sessionRepository.CreateAsync(emailSession);
                }
                if (!await IsEmailSent(paymentSession.Session.Id))
                {
                    var emailSendOperation = await _emailClient.SendAsync(
                        WaitUntil.Completed,
                        senderAddress: _senderAddress,
                        recipientAddress: toAddress,
                        subject: "Rika - Your payment was successful!",
                        htmlContent: emailContent,
                        plainTextContent: ""
                    );

                    emailSession.Sent = emailSendOperation.HasCompleted;
                    await _sessionRepository.UpdateAsync(emailSession);
                    return true;
                }
                else
                {
                    Console.WriteLine("Email has already been sent.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private static string GenerateEmailContent(PaymentSession paymentSession)
        {
            try
            {
                var productTableRows = string.Empty;
                foreach (var product in paymentSession.Session.LineItems.Data)
                {
                    productTableRows += $@"
                    <tr>
                        <td style='padding: 10px; font-size: 14px; color: #555;'>{product.Description}</td>
                        <td style='padding: 10px; font-size: 14px; color: #555;'>{product.Quantity}</td>
                        <td style='padding: 10px; font-size: 14px; color: #555;'>{paymentSession.Session.Currency.ToUpper()} {product.Price.UnitAmount / 100:F2}</td>
                    </tr>
                ";
                }
                return $@"
                <html>
                    <body style='font-family: Montserrat, sans-serif;'>
                       
                        <table role='presentation' style='width: 100%; max-width: 600px; margin: 0 auto; border-spacing: 0;'>

                            <tr>
                                <td>
                                    <h1 style='margin: 0; padding: 0;'>RIKA</h1>
                                    <h2 style='margin: 0; padding: 0;'>Online Shopping</h2>
                                </td>
                            </tr>

                            <tr>
                                <td style='padding-top: 20px;'>
                                    <h1>Your Payment was successful</h1>
                                    <p>
                                        Hey {paymentSession.Session.CustomerEmail}, <br />
                                        Thank you for your payment! Here are your order details:
                                    </p>
                                </td>
                            </tr>

                        <tr>
                            <td>
                                <h3>Order Information</h3>

                                <p>
                                    Payment date: <strong>{paymentSession.Session.Created.ToString("g")}</strong><br />
                                    Order number: <strong>{paymentSession.OrderId}</strong><br />
                                    Payment method: <strong>{paymentSession.PaymentMethod.Type} {paymentSession.PaymentMethodInfo}</strong><br />
                                    Delivery method: <strong>Some delivery method</strong><br />
                                </p>
                            </td>
                        </tr>

                     
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <h3>Customer Information</h3>

                                <p>
                                    {paymentSession.Session.CustomerDetails.Name}<br />
                                    {paymentSession.Session.CustomerDetails.Address.Line1}<br />
                                    {paymentSession.Session.CustomerDetails.Address.PostalCode} {paymentSession.Session.CustomerDetails.Address.City}<br />
                                    {paymentSession.Session.CustomerDetails.Phone}<br />
                                    {paymentSession.Session.CustomerEmail}<br />
                                </p>
                            </td>
                        </tr>

                         <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>

                         <tr>
                            <td>
                                <h3>Products</h3>      
                                
                                <table role='presentation' style='width: 100%; max-width: 600px; border-spacing: 0; margin: 0 auto; padding: 0;'>
                                    <thead>
                                        <tr>
                                            <th style='text-align: left; font-size: 14px; color: #333; border-bottom: 1px solid #ddd;'>Product</th>
                                            <th style='text-align: left; font-size: 14px; color: #333; border-bottom: 1px solid #ddd;'>Quantity</th>
                                            <th style='text-align: left; font-size: 14px; color: #333; border-bottom: 1px solid #ddd;'>Price</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {productTableRows}
                                    </tbody>
                                </table>
                            </td>
                        </tr>

                          <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>

                           <tr>
                            <td>
                                <h3>Order total</h3>

                                <p>
                                    Subtotal: {paymentSession.Session.Currency.ToUpper()} {paymentSession.Session.AmountSubtotal}<br />
                                    Shipping: {paymentSession.Session.Currency.ToUpper()} {(paymentSession.Session.ShippingCost != null ? paymentSession.Session.ShippingCost.AmountTotal : 0L)}<br />
                                    Tax: {paymentSession.Session.Currency.ToUpper()} {(paymentSession.Session.Invoice != null ? paymentSession.Session.Invoice.Tax : 0L)}<br />
                                    Total: {paymentSession.Session.Currency.ToUpper()} {paymentSession.Session.AmountTotal}<br />
                                </p>

                            </td>
                        </tr>


                         <tr>
                            <td>
                                <p>
                                    Thank you, <br />
                                    Rika
                                </p>   
                            </td>
                        </tr>

                        </table

                    </body>
                </html>    
                ";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }
    }
}