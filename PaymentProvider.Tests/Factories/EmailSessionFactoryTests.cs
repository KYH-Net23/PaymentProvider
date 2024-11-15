using PaymentProvider.Factories;
using PaymentProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentProvider.Tests.Factories
{
    public class EmailSessionFactoryTests
    {
        [Test]
        public void Create_Should_Map_All_Properties_Correctly()
        {
            // Arrange
            var expected = new EmailSession
            {
                SessionId = "TestSessionId",
                OrderId = 1,
                Sent = false,
                Date = new DateTime(2024, 11, 15)
            };

            // Act
            var actual = EmailSessionFactory.Create("TestSessionId", 1, false, new DateTime(2024, 11, 15));

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.SessionId, Is.EqualTo(expected.SessionId));
            Assert.That(actual.OrderId, Is.EqualTo(expected.OrderId));
            Assert.That(actual.Sent, Is.EqualTo(expected.Sent));
            Assert.That(actual.Date, Is.EqualTo(expected.Date));
        }
    }
}
