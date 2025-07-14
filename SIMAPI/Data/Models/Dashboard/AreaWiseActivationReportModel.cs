namespace SIMAPI.Data.Models.Dashboard
{
    public class AreaWiseActivationReportModel
    {
        public int ID { get; set; }
        public string Name { get; set; }        
        public int? PreviousActivations { get; set; }
        public int? DailyActivations { get; set; }
        public int? InstantActivations { get; set; }
        public int? Total { get; set; }
    }
}
