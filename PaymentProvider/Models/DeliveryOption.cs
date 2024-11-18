namespace PaymentProvider.Models
{
    public class DeliveryOption
    {
        public decimal Price { get; set; }
        public ServiceInformation ServiceInformation { get; set; } = null!;
        public string TimeOfArrival { get; set; } = null!;
        public string TimeOfDeparture { get; set; } = null!;
    }
}