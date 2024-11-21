using PaymentProvider.Entities;

namespace PaymentProvider.Factories.Entities;

public static class ShippingEntityFactory
{
    public static ShippingEntity Create(CustomerDeliveryInformationEntity customerDeliveryInformation, PostalAgentDeliveryInformationEntity postalAgentDeliveryInformation, DateOnly orderArrival, string trackingLink = null!)
    {
        return new ShippingEntity
        {
            CustomerDeliveryInformation = customerDeliveryInformation,
            PostalAgentDeliveryInformation = postalAgentDeliveryInformation,  
            OrderArrival = orderArrival,
            TrackingLink = trackingLink
        };
    }
}
