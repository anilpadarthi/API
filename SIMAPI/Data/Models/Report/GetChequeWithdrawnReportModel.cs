namespace SIMAPI.Data.Models.Report
{
    public class GetChequeWithdrawnReportModel
    {
        public int? AreaId { get; set; }
        public int? ShopId { get; set; }
        public int?   UserId { get; set; }
        public string? AreaName { get; set; }
        public string? ShopName { get; set; }
        public string? UserName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactName { get; set; }
        public string? ContactNumber { get; set; }
        public string? PayableName { get; set; }
        public string? VatNumber { get; set; }
        public string? ChequeNumber { get; set; }
        public string? TotalAmount { get; set; }
        public string? PaidDate { get; set; }

    }
}
