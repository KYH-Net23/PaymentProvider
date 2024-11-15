using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentProvider.Models;
using System.Text;

namespace PaymentProvider.Tests.Controllers;

public class CheckoutControllerIntegrationTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
        _client.Dispose();
    }

    [Test]
    public async Task Create_WithValidJsonBody_ReturnsCorrectSessionIdAndClientSecret()
    {
        // Arrange
        var products = new JArray
        {
            new JObject
            {
                ["id"] = 1,
                ["model"] = "T-Shirt",
                ["quantity"] = 1,
                ["price"] = 1000
            },
            new JObject
            {
                ["id"] = 2,
                ["model"] = "Boots",
                ["quantity"] = 2,
                ["price"] = 2000
            }
        };

        var orderDetails = new JObject
        {
            ["orderId"] = 10,
            ["emailAddress"] = "test@mail.se",
            ["address"] = "street 2",
            ["products"] = products
        };

        var jsonBody = orderDetails.ToString();
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/create-checkout-session", content);

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(200));

        string responseBody = await response.Content.ReadAsStringAsync();
        var checkoutResponse = JsonConvert.DeserializeObject<CheckoutResponse>(responseBody);

        Assert.That(checkoutResponse, Is.Not.Null);
        Assert.That(checkoutResponse.SessionId, Is.Not.Null);
        Assert.That(checkoutResponse.ClientSecret, Is.Not.Null);
    }

    public class CheckoutResponse
    {
        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        [JsonProperty("clientSecret")]
        public string ClientSecret { get; set; }

    }
}
