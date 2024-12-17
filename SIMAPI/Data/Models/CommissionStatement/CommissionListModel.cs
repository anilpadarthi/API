namespace SIMAPI.Data.Models.CommissionStatement
{
    public class CommissionListModel
    {
        public string UserName { get; set; }
        public string AreaName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string PostCode { get; set; }
        public DateTime CommissionDate { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal BonusAmount { get; set; }
    }
}
