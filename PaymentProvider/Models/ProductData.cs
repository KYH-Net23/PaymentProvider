using Newtonsoft.Json;

namespace PaymentProvider.Models
{
    public class ProductData
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}