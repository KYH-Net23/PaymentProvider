using PaymentProvider.Entities;
using PaymentProvider.Models;
using Stripe.Checkout;

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
    public static OrderEntity Create(OrderModel order)
    {
        try
        {
            return new OrderEntity
            {
                Shipping = order.Shipping,
                Products = order.Products.Select(productModel => new ProductEntity
                {
                    ProductId = productModel.ProductId,
                    Model = productModel.Name.Split(" - ")[0],
                    Brand = productModel.Name.Split(" - ")[1],
                    Amount = productModel.Amount,
                    Price = productModel.Price,
                    Description = productModel.Description,
                    DiscountedPrice = productModel.DiscountedPrice ?? productModel.Price,
                    Category = productModel.Category ?? "Electronics",
                    Size = productModel.Size ?? "M",
                    ImageUrl = productModel.ImageUrl
                }).ToList(),
                OrderTotal = order.OrderTotal,
                ReceivingEmail = order.ReceivingEmail,
                Date = order.Date,
                Invoice = new InvoiceEntity
                {
                    City = "",
                    Country = "",
                    FullName = "",
                    PaymentOption = "",
                    PostalCode = "",
                    StreetAddress = "",
                    PhoneNumber = "",
                    InvoiceUrl = ""
                }
            };
        }
        catch
        {
            return null!;
        }
    }
}
