using Azure;
using Azure.Communication.Email;
using PaymentProvider.Models;
using PaymentProvider.Repositories;
using Stripe.Checkout;

namespace PaymentProvider.Services
{
    public class EmailService
    {
        private readonly EmailClient _emailClient;
        private readonly EmailSessionRepository _sessionRepository;

        public EmailService(EmailSessionRepository sessionRepository, string connectionString)
        {
            _emailClient = new EmailClient(connectionString);
            _sessionRepository = sessionRepository;
        }

        public async Task<bool> IsEmailSent(string sessionId)
        {
            var session = await _sessionRepository.GetEmailSessionAsync(sessionId);
            if (session != null)
            {
                return session.Sent;
            }
            return false;
        }

        public async Task SendEmailAsync(string toAddress, Session session, OrderDetails order)
        {
            try
            {
                var emailSession = await _sessionRepository.GetEmailSessionAsync(session.Id);
                if (emailSession == null)
                {
                    emailSession = new EmailSession
                    {
                        OrderId = order.Id,
                        SessionId = session.Id,
                        Sent = false,
                        Date = DateTime.UtcNow,
                    };
                    await _sessionRepository.CreateAsync(emailSession);
                }
                if (!await IsEmailSent(session.Id))
                {
                    var emailSendOperation = await _emailClient.SendAsync(
                        WaitUntil.Completed,
                        senderAddress: "DoNotReply@e610b531-2626-468a-b39c-ee360d0cb912.azurecomm.net",
                        recipientAddress: toAddress,
                        subject: "asd",
                        htmlContent: "asd",
                        plainTextContent: "asd");
                    emailSession.Sent = emailSendOperation.HasCompleted;
                    await _sessionRepository.UpdateAsync(emailSession);
                }
                else
                {
                    Console.WriteLine("Email has already been sent.");
                    // handle more logic here?
                }
            }
            catch
            {

            }
        }
    }
}
