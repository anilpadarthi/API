
namespace SIMAPI.Data.Models.Export
{
    public class ExportTrack
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int SimsGiven { get; set; }
        public string WorkType { get; set; }
        public string Time { get; set; }
    }
}
