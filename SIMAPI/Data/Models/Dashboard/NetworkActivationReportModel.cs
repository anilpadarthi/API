namespace SIMAPI.Data.Models.Dashboard
{
    public class NetworkActivationReportModel
    {
        public string NetworkCode { get; set; }
        public DateTime? LastActivatedDate { get; set; }
        public int? PreviousActivations { get; set; }
        public int? DailyActivations { get; set; }
        public int? InstantActivations { get; set; }
        public int? Total { get; set; }
    }
}
