namespace SIMAPI.Data.Dto
{
    public class GetSimInfoRequest
    {
        public string[] imeiNumbers { get; set; }
        public int? shopId { get; set; }
        public int? loggedInUserId { get; set; }
        public string? userRole { get; set; }
    }
}
