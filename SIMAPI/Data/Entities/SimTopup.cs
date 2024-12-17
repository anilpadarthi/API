namespace SIMAPI.Data.Entities
{
    public partial class SimTopup
    {
        public int SimTopupId { get; set; }
        public int SimId { get; set; }
        public bool IsActive { get; set; }
        public DateTime TopupDate { get; set; }
        public string TopupType { get; set; }
        public decimal TopupAmount { get; set; }
        public decimal CommissionRateOnSim { get; set; }
        public decimal BonusRateOnSim { get; set; }
        public string ReportDisplayName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
