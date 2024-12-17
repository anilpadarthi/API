namespace SIMAPI.Data.Models
{
    public class PagedResult
    {
        public IEnumerable<object> Results { get; set; }
        public int TotalRecords { get; set; }
    }
}
