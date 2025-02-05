namespace SIMAPI.Data.Models.OnField
{
    public class ShopAgreementHistoryModel
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? AgreementNotes { get; set; }
        public string? AgreementBy { get; set; }
        public string? ApprovedBy { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
