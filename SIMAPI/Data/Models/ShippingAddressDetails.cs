using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models
{
    public class ShippingAddressDetails
    {
        public int ShopId { get; set; }
        public string? ShopEmail { get; set; }
        public string? ShopPhone { get; set; }
        public string? Address { get; set; }
    }
}
