namespace SIMAPI.Data.Entities
{
    public partial class Shop
    {
        public int ShopId { get; set; }
        public int? OldShopId { get; set; }
        public int AreaId { get; set; }
        public string? ShopName { get; set; }
        public string? PostCode { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Competitor { get; set; }
        public string? Language { get; set; }
        public string? PaymentMode { get; set; }
        public string? PayableName { get; set; }
        public string? City { get; set; }
        public string? DeliveryInstructions { get; set; }
        public string? Comments { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? TopupSystemId { get; set; }
        public string? Image { get; set; }
        public string? Password { get; set; }
        public string? VatNumber { get; set; }
        public short? Status { get; set; }
        public bool? IsMobileShop { get; set; }
        public bool? IsAgreeTerms { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Area? Area { get; set; }
    }
}
