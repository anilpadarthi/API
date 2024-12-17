namespace SIMAPI.Data.Entities
{
    public partial class ShopContact
    {
        public int ShopContactId { get; set; }
        public int ShopId { get; set; }

        public string? ContactName { get; set; } 

        public string? ContactEmail { get; set; }

        public string? ContactNumber { get; set; }

        public string? ContactType { get; set; }
        public short? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }
}
