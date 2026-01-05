namespace SIMAPI.Data.Models.Dashboard
{
    public class UserWiseKPIReportModel
    {
        public string? Name { get; set; }
        public int? PrevMonth { get; set; }
        public int? CurrentMonth { get; set; }
        public int? Target { get; set; }
        public string? Percentage { get; set; }
        public int? Act { get; set; }
        public decimal? Rate { get; set; }
        public int? Diff { get; set; }
        public decimal? Total { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? TotalCommission { get; set; }
    }

    public class UserWiseAccessoriesKPIReportModel
    {
        public string? Name { get; set; }
        public decimal? PrevMonth { get; set; }
        public decimal? CurrentMonth { get; set; }
        public decimal? Target { get; set; }
        public string? Percentage { get; set; }
        public decimal? Act { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Diff { get; set; }
        public decimal? Total { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? TotalCommission { get; set; }
    }
}
