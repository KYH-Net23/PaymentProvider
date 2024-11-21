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
    public static OrderEntity Create(OrderModel order, Session session)
    {
        try
        {

            return new OrderEntity
            {
                SessionId = session.Id,
                Shipping = order.Shipping,
                Products = order.Products.Select(productModel => new ProductEntity
                {
                    ProductId = productModel.ProductId,
                    Name = productModel.Name,
                    Amount = productModel.Amount,
                    Price = productModel.Price,
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
                }
            };
        }
        catch
        {
            return null!;
        }
    }
    //public static OrderEntity Create(Session session)
    //{
    //    var orderEntity = new OrderEntity
    //    {
    //        ReceivingEmail = session.Customer.Email,
    //        OrderTotal = (decimal)(session.AmountTotal! / 100m),
    //        Date = session.Created,
    //        Invoice = new InvoiceEntity
    //        {
    //            City = session.Invoice.CustomerAddress.City,
    //            Country = session.Invoice.CustomerAddress.Country,
    //            FullName = session.Invoice.CustomerName,
    //            PaymentOption = session.PaymentIntent.PaymentMethod.Card.Brand,
    //            PostalCode = session.Invoice.CustomerAddress.PostalCode,
    //            StreetAddress = session.Invoice.CustomerAddress.Line1,
    //        },
    //        Shipping = new ShippingEntity
    //        {
    //            CustomerDeliveryInformation = new CustomerDeliveryInformationEntity
    //            {
    //                FullName = session.CustomerDetails.Name,
    //                City = session.Customer.Address.City,
    //                Country = session.Customer.Address.Country,
    //                PhoneNumber = session.Customer.Phone,
    //                PostalCode = session.Customer.Address.PostalCode,
    //                StreetAddress = session.Customer.Address.Line1
    //            },
    //            PostalAgentDeliveryInformation = new PostalAgentDeliveryInformationEntity
    //            {

    //            }

    //        }
    //    };
    //}
}
