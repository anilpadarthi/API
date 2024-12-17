namespace SIMAPI.Data.Entities
{
    public partial class AreaLog
    {
        public int AreaLogId { get; set; }
        public int AreaId { get; set; }

        public string AreaName { get; set; }

        public string? AreaCode { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
