namespace SIMAPI.Data.Models.OnField
{
    public class ShopVisitHistoryModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }        
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string PostCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Comments { get; set; }
        public string WorkType { get; set; }
        public DateTime TrackedDate { get; set; }
    }
}
