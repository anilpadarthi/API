namespace SIMAPI.Data.Entities
{
    public partial class AreaMap
    {
        public int AreaMapId { get; set; }

        public int AreaId { get; set; }

        public int UserId { get; set; }

        public bool IsActive { get; set; }

        public DateTime MappedDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public virtual Area Area { get; set; } = null!;
    }
}
