namespace SIMAPI.Data.Dto
{
    public class GetLookupRequest
    {
        public int userId { get; set; }
        public int userRoleId { get; set; }
        public string filterType { get; set; }
        public int filterId { get; set; }
    }
}
