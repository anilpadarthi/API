namespace SIMAPI.Data.Entities
{
    public partial class SimMapChangeLog
    {
        public int SimMapChangeLogId { get; set; }
        public int SimId { get; set; }
        public int UserId { get; set; }
        public int ShopId { get; set; }
        public int DeAllocatedBy { get; set; }
        public DateTime MappedDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
