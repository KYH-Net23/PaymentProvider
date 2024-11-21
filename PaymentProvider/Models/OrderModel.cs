using PaymentProvider.Entities;
using PaymentProvider.Models.OrderConfirmationModels;
using System.ComponentModel.DataAnnotations;

namespace PaymentProvider.Models
{
    public class OrderModel
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(1, 10_000_000)]
        public decimal OrderTotal { get; set; }

        [Required]
        public string ReceivingEmail { get; set; } = null!;

        [Required]
        public ShippingEntity Shipping { get; set; } = null!;

        [Required]
        public List<ProductModel> Products { get; set; } = null!;
        [Required]
        public decimal ShippingPrice { get; set; }
    }
}
