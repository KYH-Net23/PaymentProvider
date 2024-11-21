using PaymentProvider.Entities;

namespace PaymentProvider.Factories.Entities;

public static class OrderEntityFactory
{
    public static OrderEntity Create(DateTime date, decimal orderTotal, string receivingEmail, ShippingEntity shipping, InvoiceEntity invoice, List<ProductEntity> products)
    {
        return new OrderEntity
        {
            Date = date,
            OrderTotal = orderTotal,
            ReceivingEmail = receivingEmail,
            Shipping = shipping,
            Invoice = invoice,  
            Products = products
        };
    }
}
