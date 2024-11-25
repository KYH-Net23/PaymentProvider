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
        [Range(0, double.MaxValue, ErrorMessage = "OrderTotal cannot be lower than 0.")]
        public decimal OrderTotal { get; set; }
        [Required]
        public string ReceivingEmail { get; set; } = null!;
        [Required]
        public ShippingEntity Shipping { get; set; } = null!;

        [Required]
        public InvoiceEntity Invoice { get; set; } = null!;

        [Required]
        public List<ProductEntity> Products { get; set; } = null!;

        public string? SessionId { get; set; }
        public ReturnEntity? Return { get; set; }
    }
}