namespace SIMAPI.Data.Models.OnField
{
    public class CommissionChequeRequestModel
    {
        public int ShopId { get; set; }
        public string ChequeNumber { get; set; }
        public string CommissionDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
