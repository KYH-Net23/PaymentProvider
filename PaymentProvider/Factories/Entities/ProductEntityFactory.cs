using PaymentProvider.Entities;

namespace PaymentProvider.Factories.Entities;

public static class ProductEntityFactory
{
    public static ProductEntity Create(string name, int amount, decimal price, decimal discountedPrice)
    {
        return new ProductEntity
        {
            Name = name,
            Amount = amount,
            Price = price,
            DiscountedPrice = discountedPrice
        };
    }
}
