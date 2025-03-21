namespace SIMAPI.Data.Models.Report
{

    public class SalaryReportModel
    {
        public IEnumerable<SalaryDetailsModel> salaryDetailsModel { get; set; }
        public IEnumerable<SalarySimCommissionDetailsModel> salarySimCommissionDetailsModel { get; set; }
        public IEnumerable<SalaryAccessoriesCommissionDetailsModel> salaryAccessoriesCommissionDetailsModel { get; set; }
        public IEnumerable<SalaryInAdvanceModel> salaryInAdvanceModel { get; set; }
    }
    public class SalaryDetailsModel
    {
        public decimal? NoOfDays { get; set; }
        public string? Description { get; set; }
        public string? SalaryType { get; set; }
        public decimal? SalaryRate { get; set; }
        public decimal? Total { get; set; }
    }

    public class SalarySimCommissionDetailsModel
    {
        public int? ActivationCount { get; set; }
        public string? NetworkName { get; set; }
        public int? KPI1Target { get; set; }
        public decimal? KPI1AchivedPercentage { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Total { get; set; }
    }

    public class SalaryAccessoriesCommissionDetailsModel
    {
        public string? SaleType { get; set; }
        public decimal? Rate { get; set; }
        public decimal? TotalSale { get; set; }
        public decimal? Total { get; set; }
    }

    public class SalaryInAdvanceModel
    {
        public int? ReferenceNumber { get; set; }
        public string? Comments { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}
