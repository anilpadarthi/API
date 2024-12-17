namespace SIMAPI.Data.Entities
{
    public partial class Area
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; } 
        public short Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<AreaMap> AreaMaps { get; } = new List<AreaMap>();
    }
}
