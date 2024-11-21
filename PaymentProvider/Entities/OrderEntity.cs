using PaymentProvider.Models.OrderConfirmationModels;
using System.ComponentModel.DataAnnotations;

namespace PaymentProvider.Entities
{
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }

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
        public InvoiceEntity Invoice { get; set; } = null!;

        [Required]
        public List<ProductEntity> Products { get; set; } = null!;        
    }
}
