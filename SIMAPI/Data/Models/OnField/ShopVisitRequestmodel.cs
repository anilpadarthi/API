namespace SIMAPI.Data.Models.OnField
{
    public class ShopVisitRequestmodel
    {
        public int? UserId { get; set; }
        public int ShopId { get; set; }
        public string? Comments { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ReferenceImage { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
    }
}
