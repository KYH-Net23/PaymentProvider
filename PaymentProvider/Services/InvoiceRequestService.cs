using PaymentProvider.Entities;
using System.Text;
using System.Text.Json;

namespace PaymentProvider.Services
{
    public class InvoiceRequestService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task CreateInvoiceAsync(InvoiceRequest invoice)
        {
            var apiUrl = "https://bankdbserver.database.windows.net/createinvoice";

            var jsonPayload = JsonSerializer.Serialize(invoice);

            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(apiUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Invoice created successfully!");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Details: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
        }
    }
}