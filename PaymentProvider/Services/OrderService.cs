using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PaymentProvider.Contexts;
using PaymentProvider.Entities;
using PaymentProvider.Models;
using Stripe.Checkout;
using Stripe.Climate;
using System;

namespace PaymentProvider.Services
{
    public class OrderService(RikaOrdersDbContext context)
    {
        private readonly RikaOrdersDbContext _context = context;

        public async Task<OrderEntity> CreateOrderAsync(OrderEntity order)
        {
            try
            {
                // Comparison logic checks to prevent duplicates

                // check if order with every detail already exists
                var existingOrder = await FindOrCreateMatchingOrderAsync(order);
                if (existingOrder != null && existingOrder != order)
                {
                    existingOrder.Date = DateTime.UtcNow;
                    if (string.IsNullOrEmpty(existingOrder.Status)) existingOrder.Status = "processing";
                    await _context.SaveChangesAsync();
                    return existingOrder;
                }

                // check if shipping details already exists
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
                // check if customers details already exists
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
                // check if postal agent details already exists
                var existingPostalAgent = await _context.PostalAgentDeliveryInformation
                    .FirstOrDefaultAsync(p => p.StreetAddress == order.Shipping.PostalAgentDeliveryInformation.StreetAddress
                    && p.PostalCode == order.Shipping.PostalAgentDeliveryInformation.PostalCode
                    && p.PostalAgentName == order.Shipping.PostalAgentDeliveryInformation.PostalAgentName
                    && p.City == order.Shipping.PostalAgentDeliveryInformation.City
                    && p.Country == order.Shipping.PostalAgentDeliveryInformation.Country
                    && p.PhoneNumber == order.Shipping.PostalAgentDeliveryInformation.PhoneNumber);
                if (existingPostalAgent != null)
                {
                    order.Shipping.PostalAgentDeliveryInformation = existingPostalAgent;
                }

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order;
            }
            catch { return null!; }
        }

        private async Task<OrderEntity> FindOrCreateMatchingOrderAsync(OrderEntity order)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.Products)
                .Include(o => o.Invoice)
                .Include(o => o.Shipping)
                .ThenInclude(s => s.CustomerDeliveryInformation)
                .Include(o => o.Shipping)
                .ThenInclude(s => s.PostalAgentDeliveryInformation)
                .FirstOrDefaultAsync(o =>
                o.Date.Date == order.Date.Date
                && o.ReceivingEmail == order.ReceivingEmail
                && o.OrderTotal == order.OrderTotal
                );
            if (existingOrder != null && AreOrdersEqual(order, existingOrder)) return existingOrder;

            return order;
        }
        public bool AreOrdersEqual(OrderEntity order1, OrderEntity order2)
        {
            if (order1 == null || order2 == null) return false;

            // status comparison
            if (order1.Status != order2.Status && order1.Status != "" && order2.Status != "") return false;

            //if (obj1.Invoice.PostalCode != obj2.Invoice.PostalCode 
            //    || obj1.Invoice.FullName != obj2.Invoice.FullName
            //    || obj1.Invoice.City != obj2.Invoice.City
            //    || obj1.Invoice.Country != obj2.Invoice.Country
            //    || obj1.Invoice.City != obj2.Invoice.City) return false;            

            // products comparison
            foreach (var product1 in order1.Products)
            {
                foreach (var product2 in order2.Products)
                {
                    if (product1.ProductId != product2.ProductId
                        || product1.Brand != product2.Brand
                        || product1.Model != product2.Model
                        || product1.Amount != product2.Amount
                        || product1.Description != product2.Description
                        || product1.Price != product2.Price
                        || product1.DiscountedPrice != product2.DiscountedPrice
                        || product1.Category != product2.Category
                        || product1.Size != product2.Size
                        || product1.ImageUrl != product2.ImageUrl) return false;
                }
            }
            // shipping.customerdelivery comparison
            if (order1.Shipping.CustomerDeliveryInformation.FullName != order2.Shipping.CustomerDeliveryInformation.FullName
                || order1.Shipping.CustomerDeliveryInformation.StreetAddress != order2.Shipping.CustomerDeliveryInformation.StreetAddress
                || order1.Shipping.CustomerDeliveryInformation.City != order2.Shipping.CustomerDeliveryInformation.City
                || order1.Shipping.CustomerDeliveryInformation.PostalCode != order2.Shipping.CustomerDeliveryInformation.PostalCode
                || order1.Shipping.CustomerDeliveryInformation.Country != order2.Shipping.CustomerDeliveryInformation.Country
                || order1.Shipping.CustomerDeliveryInformation.PhoneNumber != order2.Shipping.CustomerDeliveryInformation.PhoneNumber) return false;
            // shipping.postalagent comparison
            if (order1.Shipping.PostalAgentDeliveryInformation.PostalAgentName != order2.Shipping.PostalAgentDeliveryInformation.PostalAgentName
                || order1.Shipping.PostalAgentDeliveryInformation.StreetAddress != order2.Shipping.PostalAgentDeliveryInformation.StreetAddress
                || order1.Shipping.PostalAgentDeliveryInformation.City != order2.Shipping.PostalAgentDeliveryInformation.City
                || order1.Shipping.PostalAgentDeliveryInformation.PostalCode != order2.Shipping.PostalAgentDeliveryInformation.PostalCode
                || order1.Shipping.PostalAgentDeliveryInformation.Country != order2.Shipping.PostalAgentDeliveryInformation.Country
                || order1.Shipping.PostalAgentDeliveryInformation.PhoneNumber != order2.Shipping.PostalAgentDeliveryInformation.PhoneNumber) return false;
            // shipping comparison
            if (order1.Shipping.OrderArrival != order2.Shipping.OrderArrival
                || order1.Shipping.ShippingCost != order2.Shipping.ShippingCost) return false;
            return true;
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