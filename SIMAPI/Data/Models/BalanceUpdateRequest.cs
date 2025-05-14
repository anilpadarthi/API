namespace SIMAPI.Data.Models
{
    public class BalanceUpdateRequest
    {
        public string shop_id { get; set; }
        public string affiliate_id { get; set; }
        public decimal amount { get; set; }
    }
}
