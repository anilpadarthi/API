namespace SIMAPI.Data.Models.Report
{
    public class SimAllocationModel
    {
        public int BaseNetworkId { get; set; }
        public string BaseNetworkName { get; set; }
        public int CarryForward { get; set; }
        public int GivenToAgent { get; set; }
        public int AllocatedToShop { get; set; }
        public int TotalLeft { get; set; }
        public int LastMonthActivations { get; set; }
        public int FreeAllocations { get; set; }

    }
}
