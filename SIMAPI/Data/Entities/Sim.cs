namespace SIMAPI.Data.Entities
{
    public partial class Sim
    {
        public int SimId { get; set; }
        public string IMEI { get; set; }
        public string? PCNNO { get; set; }
        public int NetworkId { get; set; }
        public string? SupplierId { get; set; }
        public string? SupplierAccount { get; set; }
        public string? LotNo { get; set; }
        public string? SimCost { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }
}
