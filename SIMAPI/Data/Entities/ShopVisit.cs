namespace SIMAPI.Data.Entities
{
    public partial class ShopVisit
    {
        public int ShopVisitId { get; set; }
        public int ShopId { get; set; }
        public int UserId { get; set; }
        public short IsSentToWhatsApp { get; set; }

        public string Comments { get; set; } = null!;

        public string? ReferenceImage { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
