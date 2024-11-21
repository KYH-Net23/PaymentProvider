//using Microsoft.Extensions.Configuration;
//using PaymentProvider.Models.OrderConfirmationModels;
//using PaymentProvider.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PaymentProvider.Tests
//{
//    public class EmailServiceTest
//    {
//        private HttpClient _httpClient;
//        private EmailService _emailService;
//        private IConfiguration _configuration;
//        [TearDown]
//        public void Teardown()
//        {
//            _httpClient.Dispose();
//        }
//        [SetUp]
//        public void Setup()
//        {
//            _configuration = new ConfigurationBuilder()
//                .AddJsonFile("C:\\Users\\acerd\\source\\repos\\PaymentProvider\\PaymentProvider\\appsettings.json", optional: false)
//                .AddEnvironmentVariables()
//                .Build();
//            _httpClient = new HttpClient();

//            _emailService = new EmailService(_httpClient, _configuration);
//        }

//        [Test]
//        public async Task SendEmailInformationAsync_ShouldSendEmailSuccessfully()
//        {
//            // Arrange
//            var order = new OrderConfirmationModel
//            {
//                ReceivingEmail = "hossenrahimzadegan@gmail.com",
//                Shipping = new ShippingInformation
//                {
//                    FullName = "Jane Doe",
//                    CustomerDeliveryAddress = "123 Real Street",
//                    PostalPickUpAddress = "456 Pickup Street",
//                    PhoneNumber = "+46700000000",
//                    OrderArrival = DateOnly.FromDateTime(DateTime.Now.AddDays(2))
//                },
//                Invoice = new InvoiceInformation
//                {
//                    FullName = "Jane Doe",
//                    StreetAddress = "123 Real Street",
//                    City = "Testville",
//                    PostalCode = "12345",
//                    Country = "Testland",
//                    PaymentOption = "Credit Card"
//                },
//                Products = new List<ProductModel>
//                {
//                    new ProductModel
//                    {
//                        Name = "Product A",
//                        Amount = 2,
//                        Price = 50.00m,
//                        DiscountedPrice = 45.00m,
//                        ImageUrl = "https://image-link.example.com/product-a.jpg"
//                    }
//                },
//                OrderTotal = 90.00m
//            };

//            // Act
//            Assert.DoesNotThrowAsync(async () => await _emailService.SendEmailInformationAsync(order));

//            // No direct assertions for external effects; check logs or actual results in the live environment.
//        }
//    }
//}
