namespace SIMAPI.Data.Models.Tracking
{
    public class ShopVisitedModel
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Action { get; set; }
        public DateTime GivenTime { get; set; }
    }
}
