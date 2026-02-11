namespace SIMAPI.Data.Dto
{
    public class ShopCommissionChequeDto
    {
        public int ShopId { get; set; }
        public int ShopCommissionHistoryId { get; set; }
        public DateTime? CommissionDate { get; set; }
        public string? ChequeNumber { get; set; }
        public int? OrderId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaidDate { get; set; }
    }
}
