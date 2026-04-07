using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models.OrderListModels
{
    public class AgentOutstandingBalanceModel
    {
        public string? UserName { get; set; }
        public decimal? BalanceAmount { get; set; }
    }
}
