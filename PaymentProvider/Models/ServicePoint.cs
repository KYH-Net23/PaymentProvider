namespace PaymentProvider.Models
{
    public class ServicePoint
    {
        public string Name { get; set; } = null!;
        public int ServicePointId { get; set; }
        public VisitingAddress VisitingAddress { get; set; } = null!;
    }
}
