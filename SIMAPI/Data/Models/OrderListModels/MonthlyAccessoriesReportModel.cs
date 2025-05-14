
namespace SIMAPI.Data.Models.OrderListModels
{
    public class MonthlyAccessoriesReportModel
    {
       
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public decimal? TotalOrderAmount { get; set; }
        public decimal? TotalPaidAmount { get; set; }
        //public decimal? COD { get; set; }
        //public decimal? BT { get; set; }
        //public decimal? AC { get; set; }
        //public decimal? Bonus { get; set; }
        //public decimal? Free { get; set; }
        //public decimal? SaleOrReturn { get; set; }
        //public decimal? ReturnOrDamaged { get; set; }
        //public decimal? MC { get; set; }      
    }

    public class AccessoriesReportDetailModel
    {
        public string? PaymentType { get; set; }
        public decimal? TotalOrderAmount { get; set; }
        public decimal? TotalPaidAmount { get; set; }
        //public decimal? COD { get; set; }
        //public decimal? BT { get; set; }
        //public decimal? AC { get; set; }
        //public decimal? Bonus { get; set; }
        //public decimal? Free { get; set; }
        //public decimal? SaleOrReturn { get; set; }
        //public decimal? ReturnOrDamaged { get; set; }
        //public decimal? MC { get; set; }      
    }

}
