
namespace SIMAPI.Data.Models
{
    public class UserAllocationHistory
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? MonitorBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
