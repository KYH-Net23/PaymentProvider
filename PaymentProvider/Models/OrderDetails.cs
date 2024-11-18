using Stripe.Checkout;

namespace PaymentProvider.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string EmailAddress { get; set; } = null!;
        public string Address { get; set; } = null!;
        public List<ProductModel> Products { get; set; } = [];
        public List<SessionLineItemOptions> OrderItemList { get; set; } = [];
        public decimal TotalAmount { get; set; }
        public DeliveryOption DeliveryOption { get; set; } = null!;
        public ServicePoint ServicePoint { get; set; } = null!;
    }
}