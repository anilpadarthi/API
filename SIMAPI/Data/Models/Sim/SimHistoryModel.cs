namespace SIMAPI.Data.Models.Sim
{
    public class SimHistoryModel
    {
        public int? SimId { get; set; }
        public string? IMEI { get; set; }
        public string? PCNNO { get; set; }
        public string? BaseNetwork { get; set; }
        public string? NetworkName { get; set; }
        public string? Supplier { get; set; }
        public string? LotNo { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int? AreaId { get; set; }
        public string? AreaName { get; set; }
        public int? ShopId { get; set; }
        public string? ShopName { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public DateTime? Topup1Date { get; set; }
    }
}
