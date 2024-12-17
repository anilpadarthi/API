namespace SIMAPI.Data.Models.Report
{
    public class SpamReportModel: BaseNetworkCodeModel
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public DateTime ActivatedDate { get; set; }

        
    }
}
