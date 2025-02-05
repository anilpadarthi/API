namespace SIMAPI.Data.Entities
{
    public partial class UserTrack
    {
        public int UserTrackId { get; set; }
        public int UserId { get; set; }
        public int? ShopId { get; set; }
        public DateTime TrackedDate { get; set; }
        public string WorkType { get; set; }
        public string? Comments { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }   

    }
}
