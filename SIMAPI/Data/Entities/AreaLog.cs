namespace SIMAPI.Data.Entities
{
    public partial class AreaLog
    {
        public int AreaLogId { get; set; }
        public int AreaId { get; set; }

        public string AreaName { get; set; }
        public short Status { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
