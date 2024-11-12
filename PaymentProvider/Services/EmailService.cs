using Azure;
using Azure.Communication.Email;

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

        public void SendEmail(string toAddress, string subject, string body, string bodyPlainText)
        {
            EmailSendOperation emailSendOperation = _emailClient.Send(
                WaitUntil.Completed,
                senderAddress: _senderAddress,
                recipientAddress: toAddress,
                subject: subject,
                htmlContent: body,
                plainTextContent: bodyPlainText);
        }
    }
}
