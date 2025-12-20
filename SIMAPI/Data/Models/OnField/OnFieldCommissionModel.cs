namespace SIMAPI.Data.Models.OnField
{
    public class OnFieldCommissionModel: BaseNetworkCodeModel
    {
        public string Date { get; set; }
        public DateTime CommissionDate { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public int ShopCommissionHistoryId { get; set; }
        public int ShopId { get; set; }
        public string? ChequeNumber { get; set; }
        public string? OptInType { get; set; }
        public bool? IncludeWalletBonus { get; set; }
    }
}
