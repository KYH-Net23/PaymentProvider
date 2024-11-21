using PaymentProvider.Entities;

namespace PaymentProvider.Factories.Entities;

public static class ShippingEntityFactory
{
    public static ShippingEntity Create(string fullName, string customerDeliveryAddress, string postalPickUpAddress, string phoneNumber, DateOnly orderArrival, Uri trackingLink)
    {
        return new ShippingEntity
        {
            FullName = fullName,
            CustomerDeliveryAddress = customerDeliveryAddress,
            PostalPickUpAddress = postalPickUpAddress,
            PhoneNumber = phoneNumber,
            OrderArrival = orderArrival,
            TrackingLink = trackingLink
        };
    }
}
