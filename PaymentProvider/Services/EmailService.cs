using Azure;
using Azure.Communication.Email;
using PaymentProvider.Models;
using Stripe.Checkout;

namespace PaymentProvider.Services
{
    public class EmailService
    {
        private readonly EmailClient _emailClient;
        private readonly string _senderAddress;

        public EmailService(string connectionString = "endpoint=https://rika-payment-cs-26ac16f8.europe.communication.azure.com/;accesskey=y5trPSlDTyNeS7e845OImDEesENRtJdgQAn77mcjSLmuiLODNElLJQQJ99AKACULyCpqXhATAAAAAZCSgrWT", string senderAddress = "DoNotReply@e610b531-2626-468a-b39c-ee360d0cb912.azurecomm.net")
        {
            _emailClient = new EmailClient(connectionString);
            _senderAddress = senderAddress;
        }

        public void SendEmail(string toAddress, Session session)
        {
            var testProducts = new List<ProductModel>
            {
                new ProductModel { Model = "Smartphone", Quantity = 1, Price = 10000 },
                new ProductModel { Model = "TV", Quantity = 1, Price = 15000 },
            };
            var productTableRows = string.Empty;

            foreach(var product in testProducts)
            {
                productTableRows += $@"
                    <tr>
                        <td style='padding: 10px; font-size: 14px; color: #555;'>{product.Model}</td>
                        <td style='padding: 10px; font-size: 14px; color: #555;'>{product.Quantity}</td>
                        <td style='padding: 10px; font-size: 14px; color: #555;'>${product.Price:F2}</td>
                    </tr>
                ";
            }

            EmailSendOperation emailSendOperation = _emailClient.Send(
                WaitUntil.Completed,
                senderAddress: _senderAddress,
                recipientAddress: toAddress,
                subject: "Your Payment was successful",

                htmlContent: $@"
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
                                        Hey {session.CustomerEmail}, <br />
                                        Thank you for your payment! Here are your order details:
                                    </p>
                                </td>
                            </tr>

                        <tr>
                            <td>
                                <h3>Order Information</h3>

                                <p>
                                    Payment date: <strong>{session.Created.ToString("g")}</strong><br />
                                    Order number: <strong>{Guid.NewGuid().ToString()}-test</strong><br />
                                    Payment method: <strong>Card ending with...</strong><br />
                                    Delivery method: <strong>Some delivery method...</strong><br />
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
                                    John Doe<br />
                                    Testroad 10<br />
                                    10316 Stockholm<br />
                                    +46123456789<br />
                                    {session.CustomerEmail}<br />
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
                                    Subtotal: $25000<br />
                                    Shipping: Free<br />
                                    Tax: $3.06<br />
                                    Total: $25003.06<br />
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
                ",
                
                plainTextContent: "");
        }
    }
}
