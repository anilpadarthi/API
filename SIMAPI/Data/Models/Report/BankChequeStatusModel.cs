namespace SIMAPI.Data.Models.Report
{
    public class BankChequeStatusModel
    {
        public int ShopId { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? CommissionDate { get; set; }
        public string? Status { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}
