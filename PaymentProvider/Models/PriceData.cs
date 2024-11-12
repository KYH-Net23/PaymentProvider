using Newtonsoft.Json;

namespace PaymentProvider.Models
{
    public class PriceData
    {
        [JsonProperty("currency")]
        public string? Currency { get; set; }
        [JsonProperty("productData")]
        public ProductData? ProductData { get; set; }
        [JsonProperty("unitAmount")]
        public int UnitAmount { get; set; }
    }
}