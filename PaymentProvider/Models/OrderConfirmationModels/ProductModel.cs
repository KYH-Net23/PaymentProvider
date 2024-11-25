using System.ComponentModel.DataAnnotations;

namespace PaymentProvider.Models.OrderConfirmationModels
{
    public class ProductModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name must be atleast 2 characters.")]
        public string Name { get; set; } = null!;
        public int ProductId { get; set; }
        [Required]
        [Range(1, 10_000)]
        public int Amount { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be lower than 0.")]
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public string? Size { get; set; }
        public string? Description { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Discounted Price cannot be lower than 0.")]
        public decimal? DiscountedPrice { get; set; }
        [MinLength(2, ErrorMessage = "ImageUrl must be atleast 2 characters.")]
        [Url]
        public string? ImageUrl { get; set; }
    }
}