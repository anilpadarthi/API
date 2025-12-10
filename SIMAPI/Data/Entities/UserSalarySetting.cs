namespace SIMAPI.Data.Entities
{
    public partial class UserSalarySetting
    {
        public int? UserSalarySettingID { get; set; }
        public int UserId { get; set; }
        public string? SalaryBasis { get; set; }
        public string? TravelType { get; set; }
        public decimal? SalaryRate { get; set; }
        public decimal? TravelRate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
