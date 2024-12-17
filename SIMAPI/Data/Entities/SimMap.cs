namespace SIMAPI.Data.Entities
{
    public partial class SimMap
    {
        public int SimMapId { get; set; }
        public int SimId { get; set; }
        public int UserId { get; set; }
        public int ShopId { get; set; }
        public bool IsActive { get; set; }
        public DateTime MappedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
