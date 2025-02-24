
namespace SIMAPI.Data.Models
{
    public class AllocateAgentDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string? AssignedTo { get; set; }
        public int? AssignedToUserId { get; set; }
        public DateTime? FromDate { get; set; }
    }
}
