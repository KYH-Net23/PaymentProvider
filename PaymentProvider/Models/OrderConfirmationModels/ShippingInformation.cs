using PaymentProvider.Entities;
using System.ComponentModel.DataAnnotations;

namespace PaymentProvider.Models.OrderConfirmationModels
{
    public class ShippingInformation
    {

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