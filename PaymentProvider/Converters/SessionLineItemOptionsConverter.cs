using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PaymentProvider.Models;
using Stripe.Checkout;

namespace PaymentProvider.Converters
{
    public class SessionLineItemOptionsConverter : JsonConverter<SessionLineItemOptions>
    {
        public override SessionLineItemOptions ReadJson(JsonReader reader, Type objectType, SessionLineItemOptions existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            var item = new SessionLineItemOptions();
            if (obj != null)
            {

                var priceDataObj = obj["priceData"];
                if (priceDataObj != null)
                {
                    var priceData = priceDataObj.ToObject<PriceData>(serializer)!;
                    item.PriceData = new()
                    {
                        Currency = priceData.Currency,
                        UnitAmount = priceData.UnitAmount,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = priceData.ProductData?.Name,
                        }
                    };
                    item.Quantity = obj["quantity"]!.ToObject<long>(serializer);
                }
            }
            return item;
        }

        public override void WriteJson(JsonWriter writer, SessionLineItemOptions value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
