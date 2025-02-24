namespace SIMAPI.Data.Entities
{
    public partial class ShopCommissionHistory
    {
        public int ShopCommissionHistoryId { get; set; }
        public int ShopId { get; set; }
        public decimal? CommissionAmount { get; set; }
        public decimal? BonusAmount { get; set; }
        public decimal? InstantBonusAmount { get; set; }
        public string? ChequeNumber { get; set; } 
        public bool IsOptedForCheque { get; set; }
        public bool IsRedemed { get; set; }
        public bool IsBonusRedemed { get; set; }
        public bool IsInstantBonusRedemed { get; set; }
        public DateTime CommissionDate { get; set; }
        public DateTime? ChequeWithDrawDate { get; set; }

    }
}
