using System.ComponentModel.DataAnnotations.Schema;

namespace SIMAPI.Data.Entities
{
    public partial class VwPendingCommissionRequest
    {
        public int CommissionChangeRequestId { get; set; }
        public int? AreaId { get; set; }
        public int? ShopId { get; set; }
        public int? OldShopId { get; set; }
        public string? UserName { get; set; }
        public string? AreaName { get; set; }
        public string? ShopName { get; set; }
        public string? PostCode { get; set; }
        public string? Status { get; set; }
        public int? RequestedBy { get; set; }
        public int? MonitorBy { get; set; }
        public int? AreaMonitorBy { get; set; }
        public DateTime? RequestedDate { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }




    }
}
