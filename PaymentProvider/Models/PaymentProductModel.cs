namespace PaymentProvider.Models
{
    public class PaymentProductModel
    {
        public int Id { get; set; }
        public string Model { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
