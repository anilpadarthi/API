namespace SIMAPI.Data.Models.Report
{
    public class SimAllocationModel
    {
        public int BaseNetworkId { get; set; }
        public string BaseNetworkName { get; set; }
        public int AllocatedToAgent { get; set; }
        public int AllocatedToShop { get; set; }
        public int Differene { get; set; }

    }
}
