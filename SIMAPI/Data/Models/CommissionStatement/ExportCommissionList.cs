namespace SIMAPI.Data.Models.CommissionStatement
{
    public class ExportCommissionList
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? AreaName { get; set; }
        public int? ShopId { get; set; }
        public string? ShopName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? PostCode { get; set; }
        public string? PayableName { get; set; }
        public DateTime? CommissionDate { get; set; }
        public int? ReferenceNumber { get; set; }
        public bool? IsMobileShop { get; set; }
        public decimal? CommissionAmount { get; set; }
        public decimal? BonusAmount { get; set; }
    }
}
