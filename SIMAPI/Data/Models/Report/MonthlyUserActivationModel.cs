namespace SIMAPI.Data.Models.Report
{
    public class MonthlyUserActivationModel : BaseNetworkCodeModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string MonitorBy { get; set; }
    }
}
