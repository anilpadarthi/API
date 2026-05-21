using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models.OrderListModels
{
    public class AgentOutstandingBalanceModel
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public decimal? BalanceAmount { get; set; }
    }

    public class AccessoriesOutstandingBalanceModel
    {
        public string? UserName { get; set; }
        public decimal? TotalSale { get; set; }
        public decimal? TotalCollectedAmount { get; set; }
    }
}
