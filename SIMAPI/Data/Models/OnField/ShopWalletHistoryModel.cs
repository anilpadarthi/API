namespace SIMAPI.Data.Models.OnField
{
    public class ShopWalletHistoryModel
    {
        public int ShopWalletHistoryId { get; set; }
        public int UserId { get; set; }        
        public int ShopId { get; set; }
        public string TransactionType { get; set; }        
        public decimal Amount { get; set; }        
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
        public string UserName { get; set; }
        public DateTime TransactionDate { get; set; }


    }
}
