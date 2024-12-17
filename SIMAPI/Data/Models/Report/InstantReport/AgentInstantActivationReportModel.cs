namespace SIMAPI.Data.Models.Report.InstantReport
{
    public class AgentInstantActivationReportModel : BaseNetworkCodeModel
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
    }
}
