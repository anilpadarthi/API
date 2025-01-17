namespace SIMAPI.Data.Entities
{
    public partial class UserMap
    {
        public int UserMapId { get; set; }

        public int UserId { get; set; }

        public int MonitorBy { get; set; }

        public bool IsActive { get; set; }

        public DateTime MappedDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
