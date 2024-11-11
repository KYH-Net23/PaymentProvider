namespace PaymentProvider.Models
{
    public class PaymentRequest
    {
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
