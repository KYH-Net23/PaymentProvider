using PaymentProvider.Entities;
using PaymentProvider.Models.OrderConfirmationModels;

namespace PaymentProvider.Factories
{
    public static class OrderConfirmationModelFactory
    {
        public static OrderConfirmationModel Create(OrderEntity order)
        {
            return new OrderConfirmationModel
            {
                Invoice = new InvoiceInformation
                {
                    Country = order.Invoice.Country,
                    FullName = order.Invoice.FullName,
                    PaymentOption = order.Invoice.PaymentOption,
                    PostalCode = order.Invoice.PostalCode,
                    StreetAddress = order.Invoice.StreetAddress,
                    City = order.Invoice.City,
                    InvoiceUrl = order.Invoice.InvoiceUrl ?? "",
                    PhoneNumber = order.Shipping.CustomerDeliveryInformation.PhoneNumber ?? ""
                },
                Products = order.Products.Select(productModel => new ProductModel
                {
                    Amount = productModel.Amount,
                    Category = productModel.Category,
                    DiscountedPrice = productModel.DiscountedPrice,
                    ImageUrl = productModel.ImageUrl,
                    Name = productModel.Name,
                    Description = productModel.Description,
                    Price = productModel.Price,
                    ProductId = productModel.ProductId,
                    Size = productModel.Size
                }).ToList(),
                ReceivingEmail = order.ReceivingEmail,
                Shipping = new ShippingInformation
                {
                    OrderArrival = order.Shipping.OrderArrival,
                    CustomerDeliveryInformation = order.Shipping.CustomerDeliveryInformation,
                    PostalAgentDeliveryInformation = order.Shipping.PostalAgentDeliveryInformation,
                    TrackingLink = order.Shipping.TrackingLink
                },
                OrderTotal = order.OrderTotal,
            };
        }
    }
}
