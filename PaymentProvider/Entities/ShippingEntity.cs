using PaymentProvider.Models.OrderConfirmationModels;
using System.ComponentModel.DataAnnotations;

namespace PaymentProvider.Entities
{
    public class ShippingEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public CustomerDeliveryInformationEntity CustomerDeliveryInformation { get; set; } = null!;

        [Required]
        public PostalAgentDeliveryInformationEntity PostalAgentDeliveryInformation { get; set; } = null!;

        [Required]
        [FutureDate]
        public DateOnly OrderArrival { get; set; }

        [MinLength(2, ErrorMessage = "Tracking Link must be atleast 2 characters.")]
        public string? TrackingLink { get; set; }
    }
}
