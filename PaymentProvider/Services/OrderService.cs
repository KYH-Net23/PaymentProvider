using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PaymentProvider.Contexts;
using PaymentProvider.Entities;
using PaymentProvider.Models;
using Stripe.Checkout;
using Stripe.Climate;

namespace PaymentProvider.Services
{
    public class OrderService(RikaOrdersDbContext context)
    {
        private readonly RikaOrdersDbContext _context = context;

        public async Task CreateOrderAsync(OrderEntity order)
        {
            try
            {
                var existingShipping = await _context.ShippingDetails
                    .FirstOrDefaultAsync(s => s.TrackingLink == order.Shipping.TrackingLink
                    && s.OrderArrival == order.Shipping.OrderArrival
                    && s.ShippingCost == order.Shipping.ShippingCost
                    && s.CustomerDeliveryInformation == order.Shipping.CustomerDeliveryInformation
                    && s.PostalAgentDeliveryInformation == order.Shipping.PostalAgentDeliveryInformation);
                if (existingShipping != null)
                {
                    order.Shipping = existingShipping;
                }
                var existingCustomer = await _context.CustomerDeliveryInformation
                    .FirstOrDefaultAsync(c => c.StreetAddress == order.Shipping.CustomerDeliveryInformation.StreetAddress
                    && c.PostalCode == order.Shipping.CustomerDeliveryInformation.PostalCode
                    && c.City == order.Shipping.CustomerDeliveryInformation.City
                    && c.Country == order.Shipping.CustomerDeliveryInformation.Country
                    && c.FullName == order.Shipping.CustomerDeliveryInformation.FullName
                    && c.PhoneNumber == order.Shipping.CustomerDeliveryInformation.PhoneNumber);
                if (existingCustomer != null)
                {
                    order.Shipping.CustomerDeliveryInformation = existingCustomer;
                }

                //var matchedProducts = new List<ProductEntity>();
                //foreach (var product in order.Products)
                //{
                //    var matchingProduct = await _context.Products.
                //        FirstOrDefaultAsync(p => p.ProductId == product.ProductId
                //        && p.Amount == product.Amount
                //        && p.Price == product.Price
                //        && p.DiscountedPrice == product.DiscountedPrice
                //        && p.Size == product.Size);
                //    if (matchingProduct != null)
                //    {
                //        matchedProducts.Add(matchingProduct);
                //    }
                //}
                //order.Products = matchedProducts;
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            catch { }
        }

        public async Task Delete(OrderEntity order)
        {
            try
            {
                if (order == null) return;
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch { }
        }

        public async Task Delete(int id)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (order == null) return;
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch { }
        }

        public async Task<OrderEntity> GetAsync(string sessionId)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.SessionId == sessionId);
                if (order == null) return null!;
                order = await _context.Orders
                    .Include(o => o.Products)
                    .Include(o => o.Invoice)
                    .Include(o => o.Shipping)
                    .ThenInclude(s => s.CustomerDeliveryInformation)
                    .Include(o => o.Shipping)
                    .ThenInclude(s => s.PostalAgentDeliveryInformation)
                    .FirstOrDefaultAsync(o => o.Id == order.Id);
                if (order == null) return null!;
                return order;
            }
            catch { return null!; }
        }

        public async Task<OrderEntity> GetAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (order == null) return null!;
                order = await _context.Orders
                    .Include(o => o.Products)
                    .Include(o => o.Invoice)
                    .Include(o => o.Shipping)
                    .ThenInclude(s => s.CustomerDeliveryInformation)
                    .Include(o => o.Shipping)
                    .ThenInclude(s => s.PostalAgentDeliveryInformation)
                    .FirstOrDefaultAsync(o => o.Id == order.Id);
                if (order == null) return null!;
                return order;
            }
            catch { return null!; }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch { }
        }
        public List<SessionLineItemOptions> GetOrderItemsList(OrderModel order)
        {
            var lineItems = new List<SessionLineItemOptions>();
            foreach (var product in order.Products)
            {
                //order.TotalAmount += product.Price * product.Quantity;
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Name,
                        },
                        UnitAmount = (long)(product.Price * 100),
                    },
                    TaxRates =
                    [
                        "txr_1QNyV8KTnkBH3a68eOoFL3Ke"
                    ],
                    Quantity = product.Amount,
                });
            }
            return lineItems;
        }
        public SessionShippingOptionOptions GetShippingOption(OrderModel order)
        {
            if (order == null) return new SessionShippingOptionOptions
            {
                ShippingRateData = new SessionShippingOptionShippingRateDataOptions
                {
                    Type = "fixed_amount",
                    FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
                    {
                        Amount = 0,
                        Currency = "usd",
                    },
                    DisplayName = "Free shipping",
                    DeliveryEstimate = new SessionShippingOptionShippingRateDataDeliveryEstimateOptions
                    {
                        Minimum = new SessionShippingOptionShippingRateDataDeliveryEstimateMinimumOptions
                        {
                            Unit = "business_day",
                            Value = 5,
                        },
                        Maximum = new SessionShippingOptionShippingRateDataDeliveryEstimateMaximumOptions
                        {
                            Unit = "business_day",
                            Value = 7,
                        },
                    },
                }
            };
            return new SessionShippingOptionOptions
            {
                ShippingRateData = new SessionShippingOptionShippingRateDataOptions
                {
                    Type = "fixed_amount",
                    FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
                    {
                        Currency = "usd",
                        Amount = (long)order.ShippingPrice * 100,
                    },
                    TaxBehavior = "inclusive",
                    DisplayName = order.Shipping.PostalAgentDeliveryInformation.PostalAgentName,
                    DeliveryEstimate = new SessionShippingOptionShippingRateDataDeliveryEstimateOptions
                    {
                        Minimum = new SessionShippingOptionShippingRateDataDeliveryEstimateMinimumOptions
                        {
                            Unit = "business_day",
                            Value = 1
                        },
                        Maximum = new SessionShippingOptionShippingRateDataDeliveryEstimateMaximumOptions
                        {
                            Unit = "business_day",
                            Value = (order.Shipping.OrderArrival.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days + 1
                        }
                    },
                }
            };
        }
    }
}