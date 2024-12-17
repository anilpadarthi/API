namespace SIMAPI.Data.Models.Report
{
    public class MonthlyHistoryActivationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MonthWise { get; set; }
        public int TotalActivations { get; set; }
    }
}
