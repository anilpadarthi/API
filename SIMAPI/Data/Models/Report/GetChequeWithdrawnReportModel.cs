namespace SIMAPI.Data.Models.Report
{
    public class GetChequeWithdrawnReportModel
    {
        public int ShopId { get; set; }
        public string ChequeNumber { get; set; }
        public string TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CommissionDate { get; set; }

    }
}
