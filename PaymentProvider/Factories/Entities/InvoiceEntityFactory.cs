using PaymentProvider.Entities;

namespace PaymentProvider.Factories.Entities;

public static class InvoiceEntityFactory
{
    public static InvoiceEntity Create(string fullName, string streetAddress, string city, string postalCode, string country, string paymentOptions)
    {
        return new InvoiceEntity
        {
            FullName = fullName,
            StreetAddress = streetAddress,
            City = city,
            PostalCode = postalCode,
            Country = country,
            PaymentOption = paymentOptions
        };
    }
}
