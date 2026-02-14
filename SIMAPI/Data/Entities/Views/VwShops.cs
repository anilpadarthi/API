using System.ComponentModel.DataAnnotations.Schema;

namespace SIMAPI.Data.Entities
{
    public partial class VwShops
    {
        public int? AreaId { get; set; }
        public int? OldAreaId { get; set; }
        public int? ShopId { get; set; }
        public int? OldShopId { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? AreaName { get; set; }
        public string? ShopName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Password { get; set; }
        public string? PostCode { get; set; }
        public string? VatNumber { get; set; }
        public string? PayableName { get; set; }
        public string? ShopOwnerName { get; set; }
        public string? ShopEmail { get; set; }
        public string? ShopPhone { get; set; }
        public string? Status { get; set; }
        public string? PaymentMode { get; set; }
        public string? Competitor { get; set; }
        public string? Language { get; set; }
        public string? TopupSystemId { get; set; }
        public bool? IsMobileShop { get; set; }
    }
}
