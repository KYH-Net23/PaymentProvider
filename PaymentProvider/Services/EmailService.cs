using Azure;
using Azure.Communication.Email;
using PaymentProvider.Factories;
using PaymentProvider.Models;
using PaymentProvider.Models.OrderConfirmationModels;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PaymentProvider.Services
{
    public class EmailService(HttpClient client, IConfiguration config)
    {
        private readonly HttpClient _client = client;
        private readonly string _apiKey = config["PaymentEmailSecret"]!;
        private readonly string _tokenUrl = "https://rika-tokenservice-agbebvf3drayfqf6.swedencentral-01.azurewebsites.net/TokenGenerator/generate-email-token";
        private readonly string _emailUrl = "https://rika-solutions-email-provider.azurewebsites.net/OrderConfirmation";
        public async Task<string> GetBearerTokenAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _tokenUrl);

                request.Headers.Add("x-api-key", _apiKey);
                request.Headers.Add("x-provider-name", "PaymentProvider-ApiKey");
                var response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var parsedResponse = JsonDocument.Parse(jsonResponse);
                    var token = parsedResponse.RootElement.GetProperty("token").GetString();
                    return token ?? null!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null!;
            }
        }

        public async Task<bool> SendEmailInformationAsync(OrderConfirmationModel order)
        {
            try
            {
                if (order == null) return false;

                string token = await GetBearerTokenAsync();
                if (string.IsNullOrEmpty(token)) return false;
                var jsonData = JsonSerializer.Serialize(order);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_emailUrl, content);
                Console.WriteLine(response.Content.ReadAsStringAsync());
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}