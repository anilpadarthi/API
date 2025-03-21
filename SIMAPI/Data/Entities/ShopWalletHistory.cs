namespace SIMAPI.Data.Entities
{
    public partial class ShopWalletHistory
    {
        public int ShopWalletHistoryId { get; set; }
        public int UserId { get; set; }
        public int ShopId { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Comments { get; set; }
        public string? CancelledReason { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? CommissionDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string WalletType { get; set; }
        public bool IsActive { get; set; }
       
    }
}
