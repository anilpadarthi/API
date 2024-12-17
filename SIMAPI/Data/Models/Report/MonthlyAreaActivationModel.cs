namespace SIMAPI.Data.Models.Report
{
    public class MonthlyAreaActivationModel : BaseNetworkCodeModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        
        
    }
}
