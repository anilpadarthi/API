using System.ComponentModel.DataAnnotations.Schema;

namespace SIMAPI.Data.Models.CommissionStatement
{
    public class CommissionShopListModel
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int? AreaId { get; set; }
        public string? AreaName { get; set; }
        public string? AreaCode { get; set; }
        public int? ShopId { get; set; }
        public string? ShopName { get; set; }
        public string? ShopCode { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? PostCode { get; set; }
        public string? PayableName { get; set; }
        public DateTime? CommissionDate { get; set; }
        public int? ShopCommissionHistoryId { get; set; }
        public bool? IsMobileShop { get; set; }

        [NotMapped]
        public IEnumerable<CommissionStatementModel?> commissionStatementDetails { get; set; }
    }
}
