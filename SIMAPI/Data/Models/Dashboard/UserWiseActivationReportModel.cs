namespace SIMAPI.Data.Models.Dashboard
{
    public class UserWiseActivationReportModel
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public int? PreviousActivations { get; set; }
        public int? DailyActivations { get; set; }
        public int? InstantActivations { get; set; }
        public int? Total { get; set; }
    }

    public class UserWiseAccessoriesReportModel
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public decimal? AC { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? InstantBonus { get; set; }
        public decimal? COD { get; set; }
        public decimal? MC { get; set; }
        public decimal? BT { get; set; }
        public decimal? Cash { get; set; }
    }
}
