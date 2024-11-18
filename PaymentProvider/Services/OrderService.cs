using Microsoft.AspNetCore.Http;
using PaymentProvider.Models;
using Stripe.Checkout;
using Stripe.Climate;

namespace PaymentProvider.Services
{
    public class OrderService()
    {
        public List<SessionLineItemOptions> GetOrderItemsList(OrderDetails order)
        {
            var lineItems = new List<SessionLineItemOptions>();
            foreach (var product in order.Products)
            {
                order.TotalAmount += product.Price * product.Quantity;
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "sek",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Model,

                        },
                        UnitAmount = (long)product.Price,

                    },
                    Quantity = product.Quantity,
                });
            }
            return lineItems;
        }
        public SessionShippingOptionOptions GetShippingOption(ServicePoint servicePoint, DeliveryOption deliveryOption)
        {
            if (deliveryOption == null || servicePoint == null) return new SessionShippingOptionOptions
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
                        Currency = "SEK",
                        Amount = (long)deliveryOption.Price * 100,
                    },
                    DisplayName = $"{servicePoint.Name} - {deliveryOption.ServiceInformation.Name}",
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
                            Value = ((DateTime.Parse(deliveryOption.TimeOfArrival) -
                            DateTime.Parse(deliveryOption.TimeOfDeparture)).Days) + 1
                        }
                    },
                }
            };
        }
    }
}