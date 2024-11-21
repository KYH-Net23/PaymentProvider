using PaymentProvider.Entities;

namespace PaymentProvider.Factories.Entities
{
    public static class CustomerDeliveryInformationEntityFactory
    {
        public static CustomerDeliveryInformationEntity Create(string fullName, string streetAddress, string city, string postalCode, string country, string phoneNumber)
        {
            return new CustomerDeliveryInformationEntity
            {
                FullName = fullName,
                StreetAddress = streetAddress,
                City = city,
                PostalCode = postalCode,
                Country = country,
                PhoneNumber = phoneNumber
            };
        }
    }
}
