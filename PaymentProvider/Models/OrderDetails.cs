using Stripe.Checkout;

namespace PaymentProvider.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string EmailAddress { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Address2 { get; set; }
        public string Country { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<PaymentProductModel> Products { get; set; } = [];
        public List<SessionLineItemOptions> OrderItemList { get; set; } = [];
        public decimal TotalAmount { get; set; }
        public DeliveryOption DeliveryOption { get; set; } = null!;
        public ServicePoint ServicePoint { get; set; } = null!;
    }
}