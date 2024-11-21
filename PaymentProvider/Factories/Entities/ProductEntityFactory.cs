using PaymentProvider.Entities;

namespace PaymentProvider.Factories.Entities;

public static class ProductEntityFactory
{
    public static ProductEntity Create(int productId, string name, int amount, decimal price, decimal discountedPrice, string category, string size, string imageUrl = null!)
    {
        return new ProductEntity
        {
            ProductId = productId,
            Name = name,
            Amount = amount,
            Price = price,
            DiscountedPrice = discountedPrice,
            Category = category,
            Size = size,
            ImageUrl = imageUrl
        };
    }
}
