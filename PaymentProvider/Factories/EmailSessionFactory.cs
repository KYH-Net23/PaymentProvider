using PaymentProvider.Models;

namespace PaymentProvider.Factories
{
    public static class EmailSessionFactory
    {
        public static EmailSession Create(string SessionId, int OrderId, bool sent, DateTime date)
        {
            return new EmailSession
            {
                SessionId = SessionId,
                OrderId = OrderId,
                Sent = sent,
                Date = date
            };
        }
    }
}
