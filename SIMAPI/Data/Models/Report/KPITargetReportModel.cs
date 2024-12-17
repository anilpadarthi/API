namespace SIMAPI.Data.Models.Report
{
    public class KPITargetReportModel
    {
        public string Name { get; set; }
        public int LastMonthActivated { get; set; }
        public int KPI1Activations { get; set; }
        public int Achieved { get; set; }
        public decimal AchievedPercent { get; set; }
        public int Diff1 { get; set; }
        public int KPI1Bonus { get; set; }
    }
}
