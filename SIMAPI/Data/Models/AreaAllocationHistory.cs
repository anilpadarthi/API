
namespace SIMAPI.Data.Models
{
    public class AreaAllocationHistory
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string? AssignedTo { get; set; }
        public int? AssignedToUserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
