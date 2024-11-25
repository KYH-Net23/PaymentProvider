using System.ComponentModel.DataAnnotations;

namespace PaymentProvider.Entities
{
    public class ProductEntity
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Brand must be atleast 2 characters.")]
        public string Brand { get; set; } = null!;
        [Required]
        [MinLength(2, ErrorMessage = "Model must be atleast 2 characters.")]
        public string Model { get; set; } = null!;

        [Required]
        [Range(1, 10_000)]
        public int Amount { get; set; }

        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be lower than 0.")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discounted Price cannot be lower than 0.")]
        public decimal? DiscountedPrice { get; set; }

        [Required]
        public string Category { get; set; } = null!;

        [Required]
        public string Size { get; set; } = null!;

        [Url]
        public string? ImageUrl { get; set; }
    }
}