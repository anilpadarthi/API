namespace SIMAPI.Data.Entities
{
    public partial class ShopCommissionTypeHistory
    {
        public int ShopCommissionTypeHistoryId { get; set; }
        public int ShopId { get; set; }
        public int CommissionType { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
