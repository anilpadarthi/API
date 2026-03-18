namespace SIMAPI.Data.Entities
{
    public partial class ShopCommissionRequest
    {
        
        public int ShopCommissionRequestId { get; set; }
        public int ShopId { get; set; }
        public DateTime FromDate { get; set; } 
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public short IsMobileShop { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
