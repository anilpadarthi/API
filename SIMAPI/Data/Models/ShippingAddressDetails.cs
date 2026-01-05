using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models
{
    public class ShippingAddressDetails
    {
        public int ShopId { get; set; }
        public string? ShopName { get; set; }
        public string? ShopOwnerName { get; set; }
        public string? ShopEmail { get; set; }
        public string? ShopPhone { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? DeliveryInstructions { get; set; }
    }
}
