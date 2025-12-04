namespace SIMAPI.Data.Dto
{
    public class ShopCommissionChequeDto
    {
        public int ShopId { get; set; }
        public int Sno { get; set; }
        public DateTime CommissionDate { get; set; }
        public string Type { get; set; } = "Cheque";
        public string ChequeNumber { get; set; }
        public string TotalAmount { get; set; }
        public string Status { get; set; }
        public string? PaidDate { get; set; } // no date if null
    }
}
