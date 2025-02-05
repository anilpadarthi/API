using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models
{
    public class ShopAddressDetails
    {
        public int ShopId { get; set; }
        public string? ShopName { get; set; }
        public string? PostCode { get; set; }
        public string? AddressLine1 { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}
