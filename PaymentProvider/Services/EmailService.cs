﻿using Azure.Communication.Email;
using PaymentProvider.Models.OrderConfirmationModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PaymentProvider.Services
{
    public class EmailService(HttpClient client, IConfiguration config)
    {
        private readonly EmailClient _emailClient = new(config["EmailSecret"]);
        private readonly string _senderAddress = "DoNotReply@e610b531-2626-468a-b39c-ee360d0cb912.azurecomm.net";
        private readonly string _apiKey = config["PaymentEmailSecret"]!;
        private readonly HttpClient _client = client;

        public async Task<string> GetBearerTokenAsync()
        {
            var url = "https://rika-tokenservice-agbebvf3drayfqf6.swedencentral-01.azurewebsites.net/TokenGenerator/generate-email-token";

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Add("x-api-key", _apiKey);
            request.Headers.Add("x-provider-name", "PaymentProvider-ApiKey");
            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonDocument.Parse(jsonResponse);
                var token = parsedResponse.RootElement.GetProperty("token").GetString();
                return token!;
            }
            return null!;
        }

        public async Task SendEmailInformationAsync(OrderConfirmationModel order)
        {
            var url = "https://rika-solutions-email-provider.azurewebsites.net/OrderConfirmation";

            string token = await GetBearerTokenAsync();

            var jsonData = JsonSerializer.Serialize(order);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        }
    }
}