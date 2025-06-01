
namespace SIMAPI.Data.Models
{
    public class AllocateAreaDetails
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string? AssignedTo { get; set; }
        public int? AssignedToUserId { get; set; }
        public int? MonitorBy { get; set; }
        public DateTime? FromDate { get; set; }
    }
}
