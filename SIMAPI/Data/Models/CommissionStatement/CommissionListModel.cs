namespace SIMAPI.Data.Models.CommissionStatement
{
    public class CommissionListModel
    {
        public int ShopCommissionHistoryId { get; set; }
        public string UserName { get; set; }
        public string AreaName { get; set; }
        public int ShopId { get; set; }
        public int OldShopId { get; set; }
        public string ShopName { get; set; }
        public string PostCode { get; set; }
        public DateTime CommissionDate { get; set; }
        public decimal? CommissionAmount { get; set; }
        public decimal? BonusAmount { get; set; }
        public bool? IsRedemed { get; set; }
        public int? IsAllowedToRequestCheque { get; set; }
        public string? OptInType { get; set; }
        public string? TopupSystemId { get; set; }
    }
}
