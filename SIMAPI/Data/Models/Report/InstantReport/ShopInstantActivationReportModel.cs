namespace SIMAPI.Data.Models.Report.InstantReport
{
    public class ShopInstantActivationReportModel
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public string ManagerName { get; set; }
        public string SimId { get; set; }
        public string IMEI { get; set; }
        public string PCNNO { get; set; }
        public string Network { get; set; }
        public DateTime ActivatedDate { get; set; }
    }

    public class InstantActivationDetailsReportModel
    {
        public int? ShopId { get; set; }
        public string? ShopName { get; set; }
        public int? SimId { get; set; }
        public string? IMEI { get; set; }
        public string? BaseNetwork { get; set; }
        public DateTime? ActivatedDate { get; set; }
    }
}
