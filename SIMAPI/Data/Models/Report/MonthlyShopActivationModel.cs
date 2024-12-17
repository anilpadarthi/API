namespace SIMAPI.Data.Models.Report
{
    public class MonthlyShopActivationModel : BaseNetworkCodeModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
    }
}
