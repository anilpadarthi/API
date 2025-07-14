
namespace SIMAPI.Data.Models.Export
{
    public class ExportShop
    {
        public int? AreaId { get; set; }
        public int? ShopId { get; set; }
        public string? ShopName { get; set; }
        public string? PostCode { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Password { get; set; }
        public string? VatNumber { get; set; }
        public string? Status { get; set; }
        public string? PaymentMode { get; set; }
        public string? PayableName { get; set; }
        public string? Competitor { get; set; }
        public string? Language { get; set; }
        public string? TopupSystemId { get; set; }
        public bool? IsMobileShop { get; set; }
    }
}
