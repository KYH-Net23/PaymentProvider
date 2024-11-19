using System.ComponentModel.DataAnnotations;

namespace PaymentProvider.Entities
{
    public class ProductEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Name must be atleast 2 characters.")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(1, 10_000)]
        public int Amount { get; set; }

        [Required]
        [Range(1, 100_000)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 100_000)]
        public decimal? DiscountedPrice { get; set; }
    }
}
