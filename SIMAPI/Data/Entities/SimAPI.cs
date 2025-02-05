namespace SIMAPI.Data.Entities
{
    public partial class SimAPI
    {
        public int Sno { get; set; }
        public int? SimId { get; set; }
        public int? ShopId { get; set; }
        public int? AreaId { get; set; }
        public int? NetworkId { get; set; }
        public string? NetworkName { get; set; }
        public string? BaseNetwork { get; set; }
        public string? AreaName { get; set; }
        public string? ShopName { get; set; }
        
        public DateTime? AssignedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public int? AssignedToShopByUserId { get; set; }
        public bool? IsInstantActivation { get; set; }

        public DateTime? SimUploadedDate { get; set; }
        public string? IMEI { get; set; }
        public string? PCNNO { get; set; }
        public string? Supplier { get; set; }
        public string? LotNo { get; set; }


    }
}
