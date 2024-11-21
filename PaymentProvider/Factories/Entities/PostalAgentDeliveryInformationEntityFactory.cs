using PaymentProvider.Entities;

namespace PaymentProvider.Factories.Entities
{
    public static class PostalAgentDeliveryInformationEntityFactory
    {
        public static PostalAgentDeliveryInformationEntity Create(string postalAgentName, string streetAddress, string city, string postalCode, string country, string phoneNumber)
        {
            return new PostalAgentDeliveryInformationEntity()
            {
                PostalAgentName = postalAgentName,
                StreetAddress = streetAddress,
                City = city,
                PostalCode = postalCode,
                Country = country,
                PhoneNumber = phoneNumber
            };
        }
    }
}
