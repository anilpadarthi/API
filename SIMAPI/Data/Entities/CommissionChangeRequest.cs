namespace SIMAPI.Data.Entities
{
    public partial class CommissionChangeRequest
    {        
        public int CommissionChangeRequestId { get; set; }
        public int ShopId { get; set; }
        public int CurrentCommissionType { get; set; }
        public int RequestedCommissionType { get; set; }
        public int Status { get; set; }
        public int RequestedBy { get; set; }
        public DateTime? RequestedDate { get; set; } 
        public DateTime? EffectiveFromDate { get; set; } 
        public DateTime? EffectiveToDate { get; set; }
        public string Remarks { get; set; }
        public string ManagerRemarks { get; set; }
        public string AdminRemarks { get; set; }
        public int ManagerId { get; set; }
        public int? AdminId { get; set; }
        public DateTime ManagerDecisionDate { get; set; }
        public DateTime? AdminDecisionDate { get; set; }
    }
}
