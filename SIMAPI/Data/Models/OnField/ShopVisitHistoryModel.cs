namespace SIMAPI.Data.Models.OnField
{
    public class ShopVisitHistoryModel
    {
        public string UserName { get; set; }        
        public string ShopName { get; set; }
        public string? PostCode { get; set; }       
        public string? Comments { get; set; }
        public string? WorkType { get; set; }
        public string? TrackedDate { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}
