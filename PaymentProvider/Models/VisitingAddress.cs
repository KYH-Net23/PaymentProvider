namespace PaymentProvider.Models
{
    public class VisitingAddress
    {
        public string City { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string StreetName { get; set; } = null!;
        public string StreetNumber { get; set; } = null!;
    }
}