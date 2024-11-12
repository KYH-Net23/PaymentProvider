namespace PaymentProvider.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public List<ProductModel> Products { get; set; } = [];
        public decimal TotalAmount { get; set; }
    }
}
